using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vid
{
    public partial class HistoryForm : Form
    {
        GameList gameList;

        public HistoryForm()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            gameList = GameList.NewInstance();

            foreach (var game in gameList)
            {
                // Adding game history details to list view
                ListViewItem gameHistory = new ListViewItem(game.id + 1 + "");  // To start counting from 1
                gameHistory.SubItems.Add(game.date + "");
                gameHistory.SubItems.Add(game.Team1 + "");
                gameHistory.SubItems.Add(game.Team1Score + "");
                gameHistory.SubItems.Add(game.Team2 + "");
                gameHistory.SubItems.Add(game.Team2Score + "");

                listView1.Items.Add(gameHistory);
            }
        }
    }
}
