/* 
 * nPuzzleGameForm.cs
 * Assignment 3
 * Revision History,
 *      Janki Joshi, 2016.10.24: designed
 *      Janki Joshi, 2016.10.26: created
 *      Janki Joshi, 2016.10.27: created[continued]
 *      Janki Joshi, 2016.10.28: created[continued]
 *      Janki Joshi, 2016.10.29: created[continued]
 *      Janki Joshi, 2016.10.30: design [modified], added comments, debugged                               
 *                               
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// the starting of the program
/// </summary>
namespace JJoshiAssignment3
{   
    /// <summary>
    /// --> class creates the nPuzzle game with the help of buttons
    /// and various methods are created to handle the game. 
    /// --> 2 solvable text files are zipped as not all the random generated games are solvable!
    /// </summary>
    public partial class nPuzzleGameForm : Form
    {
        // button list stores all the buttons with all its properties
        List<Button> puzzleButton = new List<Button>();

        int staticRow = 0;
        int staticColumn = 0;

        Button buttonClicked; 
                   
        int left = 70;
        int top;

        Button free;

        /// <summary>
        /// default constructor of the class that initializes the form and all components on it.
        /// </summary>
        public nPuzzleGameForm()
        {
            InitializeComponent();            
        }        


        /// <summary>
        /// this click event calls the generateButton method to generate the tiles and clear tiles 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {            
            foreach (Button item in puzzleButton)
            {
                item.Click -= pButton_Click;
                this.Controls.Remove(item);                
                item.Dispose();               
            }

            top = 130;
            puzzleButton.Clear();                     

            int row = int.Parse(txtRow.Text);
            int column = int.Parse(txtColumn.Text);

            staticRow = row;
            staticColumn = column;

            generateButtons(row, column);         
            
        }


        /// <summary>
        /// generate tiles at a particular loaction on form
        /// </summary>
        /// <param name="row">specifies the number of rows</param>
        /// <param name="column">specifies the number of columns</param>
        void generateButtons(int row, int column)
        {
            int total = row * column;      

            List<int> list = new List<int>();
            Random randomNumber = new Random();
            int random = randomNumber.Next(1, total + 1);
            list.Add(random);
            int count = 0;
            int print = 0;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    // calling the button class
                    Button t = new Button();

                    // it is tracking the index for the loop
                    print++;

                    do
                    {
                        random = randomNumber.Next(1, total + 1);
                        if (!list.Contains(random))
                        {
                            list.Add(random);
                        }
                        count++;
                    } while (count <= 10 * total);

                    t.Left = left;
                    t.Top = top;
                    t.Width = 60;
                    t.Height = 60;
                    t.Text = list[print - 1].ToString();
                    t.BackColor = Color.LightPink;
                    t.Font = new Font(t.Font, FontStyle.Bold);
                    t.Name = t.Text;
                    puzzleButton.Add(t);

                    if (int.Parse(t.Text) == total)
                    {
                        t.Visible = false;
                        free = t;
                    }
                    this.Controls.Add(t);
                    t.Click += pButton_Click;
                    left += 60;
                }
                top += 60;
                left = 70;
            }
        }        

        /// <summary>
        /// this button click event checks the empty slot of tile and swaps the tiles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void pButton_Click(object sender, EventArgs e)
        {
            buttonClicked = (Button)sender;
            const int top = 60;
            const int left = 60;

            int total = puzzleButton.Count;

            int clickedIndex;
            int freeIndex = 0;
            
            // empty slot at top of the button Clicked
            if (buttonClicked.Left == free.Left && free.Top + free.Height == buttonClicked.Top)
            {
                buttonClicked.Top -= top;
                free.Top += top;
                clickedIndex = puzzleButton.IndexOf(buttonClicked);
                freeIndex = puzzleButton.IndexOf(free);
                swapButtons(puzzleButton, free, clickedIndex, freeIndex);
                //freeIndex = int.Parse(free.Text);
                checkWinner(total, puzzleButton, free);                      
            }

            // empty slot at right of the button Clicked
            else if (buttonClicked.Top == free.Top && buttonClicked.Left + buttonClicked.Width == free.Left)
            {
                buttonClicked.Left += left;
                free.Left -= left;
                clickedIndex = puzzleButton.IndexOf(buttonClicked);
                freeIndex = puzzleButton.IndexOf(free);
                swapButtons(puzzleButton, free, clickedIndex, freeIndex);
                //freeIndex = int.Parse(free.Text);
                checkWinner(total, puzzleButton, free);
            }

            // empty slot at left of the button Clicked
            else if (buttonClicked.Top == free.Top && buttonClicked.Left == free.Left + free.Width)
            {
                buttonClicked.Left -= left;
                free.Left += left;
                clickedIndex = puzzleButton.IndexOf(buttonClicked);
                freeIndex = puzzleButton.IndexOf(free);
                swapButtons(puzzleButton, free, clickedIndex, freeIndex);                
                checkWinner(total, puzzleButton, free);
            }

            // empty slot at bottm of the button Clicked
            else
            {
                if (buttonClicked.Left == free.Left && buttonClicked.Top + buttonClicked.Height == free.Top)
                {
                    buttonClicked.Top += top;
                    free.Top -= top;
                    clickedIndex = puzzleButton.IndexOf(buttonClicked);
                    freeIndex = puzzleButton.IndexOf(free);
                    swapButtons(puzzleButton, free, clickedIndex, freeIndex);                    
                    checkWinner(total, puzzleButton, free);
                }
            }                
        }        

        /// <summary>
        /// this method swaps the tiles and arranges the list
        /// </summary>
        /// <param name="puzzleButton"> button list </param>
        /// <param name="free">empty slot button</param>
        /// <param name="clickedIndex">index of the button that is clicked</param>
        /// <param name="freeIndex">index of the current empty slot</param>
        void swapButtons(List<Button> puzzleButton, Button free, int clickedIndex, int freeIndex)
        {
            Button swapHelp;
            swapHelp = puzzleButton[clickedIndex];
            puzzleButton[clickedIndex] = puzzleButton[freeIndex];            
            puzzleButton[freeIndex] = swapHelp;            
        }


        /// <summary>
        /// this method checks the winner
        /// </summary>
        /// <param name="total">total buttons on the form</param>
        /// <param name="puzzleButton">button list</param>
        /// <param name="free">empty slot button</param>
        void checkWinner(int total, List<Button> puzzleButton, Button free)
        {
            if (puzzleButton[0].Text == "1")
            {                
                for (int i = 1; i < total; i++)
                {                                        
                    if (int.Parse(puzzleButton[i - 1].Text) > int.Parse(puzzleButton[i].Text))
                    {
                        return;
                    }
                }
                MessageBox.Show("Great Job. You Win");              
            }
        }

        /// <summary>
        /// this click event calls the doload method to load the current game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuLoad_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.Abort:
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    string filename = openFileDialog.FileName;
                    doLoad(filename);
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Yes:
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// saves the current game
        /// </summary>
        /// <param name="filename"></param>
        private void doSave(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);

            writer.WriteLine(txtRow.Text);
            writer.WriteLine(txtColumn.Text);            

            foreach (Button item in puzzleButton)
            {               
                writer.WriteLine(item.Text);
            }

            writer.Close();
                        
        }


        /// <summary>
        /// loads the current game
        /// </summary>
        /// <param name="filename"></param>
        private void doLoad(string filename)
        {
            StreamReader reader = new StreamReader(filename);

            int min = int.Parse(reader.ReadLine());
            int max = int.Parse(reader.ReadLine());

            List<int> listCheck = new List<int>();

            int n = min * max;

            for (int i = 0; i < n; i++)
            {
                int item = int.Parse(reader.ReadLine());
                listCheck.Add(item);
            }

            loadSavedGame(min, max, listCheck);          
            reader.Close();
        }


        /// <summary>
        /// this click event calls the doSave method to save the current game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSave_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.Abort:
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    try
                    {
                        string filename = saveFileDialog.FileName;
                        doSave(filename);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error in file save");
                    }
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Yes:
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// this method iniializes all the components based on the saved game.
        /// </summary>
        /// <param name="min">it is number of rows saved</param>
        /// <param name="max">it is the number of columns saved</param>
        /// <param name="listCheck">the list<int> having all the nmbers saved in the current format</int></param>
        void loadSavedGame(int min, int max, List<int> listCheck)
        {
            foreach (Button item in puzzleButton)
            {
                item.Click -= pButton_Click;
                this.Controls.Remove(item);
                item.Dispose();
            }

            txtRow.Text = min.ToString();
            txtColumn.Text = max.ToString();

            top = 130;
            puzzleButton.Clear();

            int total = min * max;

            int row = min;
            int column = max;
            
            int print = 0;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    // calling the button class
                    Button t = new Button();

                    // it is tracking the index for the loop
                    print++;                    

                    t.Left = left;
                    t.Top = top;
                    t.Width = 60;
                    t.Height = 60;
                    t.Text = listCheck[print-1].ToString();
                    t.BackColor = Color.LightPink;
                    t.Name = t.Text;
                    puzzleButton.Add(t);

                    if (int.Parse(t.Text) == total)
                    {
                        t.Visible = false;
                        free = t;
                    }
                    this.Controls.Add(t);
                    t.Click += pButton_Click;
                    left += 60;
                }
                top += 60;
                left = 70;
            }

        }

        /// <summary>
        /// exits the application when pressed OK!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit application?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }

}

