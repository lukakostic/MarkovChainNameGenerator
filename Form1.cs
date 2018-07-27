using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace MarkovChainNameGenerator
{
    public partial class Form1 : Form
    {
        public LetterProbability startLetter;
        Random r;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MarkovChainNameGenerator.Resources.Names.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                richTextBox1.Text = result;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i2 = 0; i2 < (int)(numericUpDown1.Value); i2++)
            {
                LetterProbability lp = startLetter;
                for (int i = 0; i < (int)(numericUpDown2.Value); i++)
                {
                    lp = getRandomLetter(lp);
                    while (lp == null)
                    {
                        lp = startLetter;
                        lp = getRandomLetter(lp);
                    }
                    sb.Append(lp.letter);
                }
                sb.Append(Environment.NewLine);
            }

            richTextBox2.Text = sb.ToString();
        }

        public LetterProbability getRandomLetter(LetterProbability previous)
        {
            int i = r.Next(previous.NextLetters.Count);
            try
            {
                
                return previous.NextLetters[i];

            }catch(Exception ex) {
                try
                {
                    var start = r.Next( startLetter.NextLetters.Count);
                    return startLetter.NextLetters[start].NextLetters[r.Next(startLetter.NextLetters[start].NextLetters.Count)];
                }
                catch (Exception exx) { }
            }

            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LetterProbability last = null;
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                last = startLetter;
                for (int i2 = 0; i2 < richTextBox1.Lines[i].Length; i2++)
                {
                        last = AddLetterProbability(last, richTextBox1.Lines[i][i2]);
                }
            }

            MessageBox.Show("Learnt.");
        }

        public LetterProbability AddLetterProbability(LetterProbability lp, char ch)
        {
            LetterProbability found = null;

                found = new LetterProbability(ch);
                lp.NextLetters.Add(found);
   
            return found;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startLetter = new LetterProbability('.');
            r = new Random();
        }
    }
    public class LetterProbability
    {
        public char letter;
        public List<LetterProbability> NextLetters;

        public LetterProbability(char c)
        {
            letter = c;
            NextLetters = new List<LetterProbability>();
        }
    }
}
