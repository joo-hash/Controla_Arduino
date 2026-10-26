using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.IO;
using System.IO.Ports;

namespace Arduino
{
    public partial class ArduinoIHM : Form
    {
        public ArduinoIHM()
        {
            InitializeComponent();

            serialPort1 = new SerialPort("COM5", 9600);
            serialPort1.NewLine = "\n";

            try
            {
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir a porta serial: " + ex.Message);
            }
        }

        private ArduinoDAL arduinoDAL = new ArduinoDAL();
        private SerialPort serialPort1;
        private void EnviarEstado()
        {
            if (serialPort1 == null || !serialPort1.IsOpen)
            {
                throw new InvalidOperationException("A porta serial não está conectada.");
            }
            int estado = ArduinoBLL.getDisplay();

            serialPort1.WriteLine(estado.ToString());
        }

        private void tratarBotoes(object sender, EventArgs e)
        {
            Button generico = (Button)sender;
            string dispositivo = generico.Tag.ToString();

            int estadoAnterior = ArduinoBLL.getDisplay();
            bool ligar = generico.Text == "Ligar";

            try
            {
                if (ligar) { ArduinoBLL.ligaDispositivo(dispositivo); }
                else { ArduinoBLL.desligaDispositivo(dispositivo); }
                    
                EnviarEstado();
                arduinoDAL.Registrar(dispositivo,ligar ? "LIGAR" : "DESLIGAR");
                generico.Text = ligar ? "Desligar" : "Ligar";
                textBox1.Text = ArduinoBLL.mostraBits(ArduinoBLL.getDisplay());
            }
            catch (Exception ex)
            {
                ArduinoBLL.setDisplay(estadoAnterior);
                MessageBox.Show("Erro ao controlar o dispositivo: " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                // Zera o display e Envia o estado 0 para o Wokwi via serial
                ArduinoBLL.setDisplay(0);
                EnviarEstado();

                // Atualiza a Bits Mostrados e textos dos botões
                textBox1.Text = ArduinoBLL.mostraBits(ArduinoBLL.getDisplay());
                button1.Text = "Ligar";
                button2.Text = "Ligar";
                button3.Text = "Ligar";
                button4.Text = "Ligar";
                button5.Text = "Ligar";
                button6.Text = "Ligar";
                button7.Text = "Ligar";
                button8.Text = "Ligar";

                // Registra a ação no banco de dados para cada dispositivo
                for (int i = 1; i <= 8; i++)
                {
                    arduinoDAL.Registrar(i.ToString(), "DESLIGAR");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao desligar todos os dispositivos: " + ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1 == null || !serialPort1.IsOpen)
                {
                    MessageBox.Show("A porta COM5 não está aberta.");
                    return;
                }

                // Define o display como 255 (todos os 8 bits ligados: 11111111)
                ArduinoBLL.setDisplay(255);

                // Envia o estado atualizado para a serial
                EnviarEstado();

                // Atualiza a interface gráfica (Bits e textos dos botões)
                textBox1.Text = ArduinoBLL.mostraBits(ArduinoBLL.getDisplay());
                button1.Text = "Desligar";
                button2.Text = "Desligar";
                button3.Text = "Desligar";
                button4.Text = "Desligar";
                button5.Text = "Desligar";
                button6.Text = "Desligar";
                button7.Text = "Desligar";
                button8.Text = "Desligar";

                // Registra a ação no banco de dados para cada dispositivo
                for (int i = 1; i <= 8; i++)
                {
                    arduinoDAL.Registrar(i.ToString(), "LIGAR");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na comunicação: " + ex.Message);
            }
        }

        private void ArduinoIHM_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1 != null && serialPort1.IsOpen) { serialPort1.Close(); }
        }

    }
}
