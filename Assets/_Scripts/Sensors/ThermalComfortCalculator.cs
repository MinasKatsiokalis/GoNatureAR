using System;

namespace GoNatureAR.Sensors
{
    public static class ThermalComfortCalculator
    {
        /// <summary>
        /// Returns Predicted Mean Vote (PMV) only by given <paramref name="rh"/> and <paramref name="tdb"/>.
        /// Parameters
        /// -----------
        /// tdb : float or array-like dry bulb air temperature, default in [°C]
        /// tr  : float or array-like mean radiant temperature, default in [°C]
        /// vr  : float or array-like relative air speed, default in [m/s]
        /// rh  : float or array-like relative humidity, [%]
        /// met : float or array-like metabolic rate, [met]
        /// clo : float or array-like clothing insulation, [clo]
        /// wme : float or array-like external work, [met] default 0
        /// ------------
        /// The ISO 7730 2005 limits are 10 < tdb [°C] < 30, 10 < tr [°C] < 40,
        /// 0 < vr [m/s] < 1, 0.8 < met [met] < 4, 0 < clo [clo] < 2, and -2 < PMV < 2.
        /// ------------
        /// Formula adjusted by the python package "pythermalcomfort".
        /// Reference link: <see cref="https://www.softxjournal.com/article/S2352-7110(20)30291-0/fulltext"/>
        /// </summary>
        /// <param name="tdb"></param>
        /// <param name="rh"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static double CalculatePMV(double airTemperature, double relativeHumidity)
        {
            double tr = airTemperature;
            double vr = 0.1;
            double met = 1.2;
            double clo = 1;
            double wme = 0;

            double pa = relativeHumidity * 10 * Math.Exp(16.6536 - 4030.183 / (airTemperature + 235));

            double icl = 0.155 * clo; // thermal insulation of the clothing in M2K/W
            double m = met * 58.15; // metabolic rate in W/M2
            double w = wme * 58.15; // external work in W/M2
            double mw = m - w; // internal heat production in the human body

            // calculation of the clothing area factor
            double f_cl = icl <= 0.078 ? 1 + (1.29 * icl) : 1.05 + (0.645 * icl);

            // heat transfer coefficient by forced convection
            double hcf = 12.1 * Math.Sqrt(vr);
            double hc = hcf; // initialize variable
            double taa = airTemperature + 273;
            double tra = tr + 273;
            double t_cla = taa + (35.5 - airTemperature) / (3.5 * icl + 0.1);

            double p1 = icl * f_cl;
            double p2 = p1 * 3.96;
            double p3 = p1 * 100;
            double p4 = p1 * taa;
            double p5 = (308.7 - 0.028 * mw) + (p2 * Math.Pow(tra / 100.0, 4));
            double xn = t_cla / 100;
            double xf = t_cla / 50;
            const double eps = 0.00015;

            int n = 0;
            while (Math.Abs(xn - xf) > eps)
            {
                xf = (xf + xn) / 2;
                double hcn = 2.38 * Math.Pow(Math.Abs(100.0 * xf - taa), 0.25);
                hc = hcf > hcn ? hcf : hcn;
                xn = (p5 + p4 * hc - p2 * Math.Pow(xf, 4)) / (100 + p3 * hc);
                n++;
                if (n > 150)
                {
                    throw new InvalidOperationException("Max iterations exceeded");
                }
            }

            double tcl = 100 * xn - 273;

            // heat loss diff. through skin
            double hl1 = 3.05 * 0.001 * (5733 - (6.99 * mw) - pa);
            // heat loss by sweating
            double hl2 = mw > 58.15 ? 0.42 * (mw - 58.15) : 0;
            // latent respiration heat loss
            double hl3 = 1.7 * 0.00001 * m * (5867 - pa);
            // dry respiration heat loss
            double hl4 = 0.0014 * m * (34 - airTemperature);
            // heat loss by radiation
            double hl5 = 3.96 * f_cl * (Math.Pow(xn, 4) - Math.Pow(tra / 100.0, 4));
            // heat loss by convection
            double hl6 = f_cl * hc * (tcl - airTemperature);

            double ts = 0.303 * Math.Exp(-0.036 * m) + 0.028;
            double pmv = ts * (mw - hl1 - hl2 - hl3 - hl4 - hl5 - hl6);

            return Math.Round(pmv, 2);
        }

        /// <summary>
        /// Calculates Predicted Percentage of Dissatisfied (PPD)
        /// </summary>
        /// <param name="pmv"></param>
        /// <returns></returns>
        public static int CalculatePPD(double pmv)
        {
            return (int)(100.0 - 95.0 * Math.Exp(-0.03353 * Math.Pow(pmv, 4) - 0.2179 * Math.Pow(pmv, 2)));
        }

        public static ThermalComofortIndex GetThermalComfortIndex(double pmv)
        {
            if (pmv > 1.5)
                return ThermalComofortIndex.Hot;
            else if (pmv < -1.5)
                return ThermalComofortIndex.Cold;
            else
                return ThermalComofortIndex.Comfortable;
        }
    }
}
