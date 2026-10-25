using System;

namespace Arduino
{
    class ArduinoBLL
    {
        private static int display = 0;

        public static void setDisplay(int _display) { display = _display; }
        public static int getDisplay() { return display; }

        public static String mostraBits(int _x)
        {
            String retorno = "";
            int aux = 128;

            for (int i = 0; i < 8; ++i)
            {
                if ((_x & aux) != 0)
                    retorno += "1";
                else
                    retorno += "0";
                aux = aux >> 1;
            }
            return retorno;
        }

        public static void ligaDispositivo(String _n)
        {
            int aux = 1 << (int.Parse(_n) - 1);
            display = display | aux;
        }

        public static void desligaDispositivo(String _n)
        {
            int aux = 1 << (int.Parse(_n) - 1);
            display = display & ~aux;
        }

    }
}
