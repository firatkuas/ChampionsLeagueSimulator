using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChampionsLeagueSimUI
{
    public partial class Form1 : Form
    {
        //Tanımlar
        ITeamService teams = new TeamManager(new EfTeamDal());
        IMatchService matchService = new MatchManager(new EfMatchDal());
        IPointStateService pointStateService = new PointStateManager(new EfPointStateDal());
        List<PointStateDetailDto> filteredData = new List<PointStateDetailDto>();
        Random random = new Random();
        string checkedGroups = "ABCDEFGH";
        byte pocketId = 0;
        byte groupId = 64;
        public Form1()
        {
            InitializeComponent();
        }
        private void RefreshDataGrids()
        {


            //DB'den verileri çek
            var data = teams.GetTeamDetails();

            List<DataGridView> list = new List<DataGridView>
            {
                dgvA,dgvB,dgvC,dgvD,dgvE,dgvF,dgvG,dgvH,dgvTeams
            };


            //Gridleri Listeye Ata
            //foreach (Control item in tabGroupTeams.TabPages[0].Controls)
            //{

            //    if (item.GetType().Name == "DataGridView")
            //    {
            //        list.Add((DataGridView)item);
            //    }
            //}
            //Grid verileri sil
            foreach (var item in list)
            {
                item.DataSource = "";
            }
            //Gridleri güncelle
            this.dgvTeams.DataSource = data.Where(d => d.GroupId == ' ').ToList();
            dgvA.DataSource = data.Where(d => d.GroupId == 'A').ToList();
            dgvB.DataSource = data.Where(d => d.GroupId == 'B').ToList();
            dgvC.DataSource = data.Where(d => d.GroupId == 'C').ToList();
            dgvD.DataSource = data.Where(d => d.GroupId == 'D').ToList();
            dgvE.DataSource = data.Where(d => d.GroupId == 'E').ToList();
            dgvF.DataSource = data.Where(d => d.GroupId == 'F').ToList();
            dgvG.DataSource = data.Where(d => d.GroupId == 'G').ToList();
            dgvH.DataSource = data.Where(d => d.GroupId == 'H').ToList();
            //Görünmesini istemediğim alanlar
            foreach (var item in list)
            {
                item.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
                item.Columns[0].Visible = false; //idleri sakla
                item.Columns[4].Visible = false; //grupları sakla
                item.Columns[2].Visible = false; //Uzun Ülke ismi
                item.Columns[5].Visible = false;
            }
            dgvTeams.Columns[2].Visible = true;
            dgvTeams.Columns[5].Visible = true;

            dgvTeams.Refresh();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tabGroupTeams.TabPages.Clear();
            tabGroupTeams.TabPages.Add("Group Teams");
            tabGroupTeams.TabPages.Add("Fixture");
            tabGroupTeams.TabPages[1].Controls.Add(grpBoxFixture);

            tabGroupTeams.TabPages[0].Controls.Add(grpBoxGroupTeams);
            DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
            dataGridViewCellStyle.BackColor = Color.LightGreen;
            foreach (var item in grpBoxGroupTeams.Controls)
            {
                if (item.GetType() == typeof(DataGridView))
                {
                    DataGridView dgv = (DataGridView)item;
                    dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
                }

            }
            //dgvFixture.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
            //dgvPointStates.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
            RefreshDataGrids();

            //var showFirst = (dgvTeams.RowCount == 0) ? "Tüm Takımlar Gruplandı!" : "Takımlar Henüz Gruplanmadı!";
            //MessageBox.Show(showFirst);

            grpBoxGroupTeams.Text = "Group Teams";
            this.Text = "Champions League Similator V 1.0";

            GetFixture();

        }

        private void btnPullOut_Click(object sender, EventArgs e)
        {
            if (pocketId == 4) pocketId = 1; else pocketId++;
            var allTeamsByGroupId = teams.GetAll().Where(g => g.GroupId == ' ').ToList();
            var selectedTeamsByPocketId = allTeamsByGroupId.Where(t => t.PocketId == pocketId).ToList();
            if (selectedTeamsByPocketId.Count < 1)
            {
                btnPullOut.Enabled = false;
            }
            else
            {
                var selecteDTeam = selectedTeamsByPocketId[random.Next(selectedTeamsByPocketId.Count)];
                selecteDTeam.GroupId = (allTeamsByGroupId.Count % 4 == 0) ? (char)(++groupId) : (char)groupId;

                if ((teams.GetAll().FirstOrDefault(c => c.CountryId == selecteDTeam.CountryId & c.GroupId == selecteDTeam.GroupId) is null))
                {
                    teams.Update(selecteDTeam);
                }
                else
                {
                    MessageBox.Show("Seçilen takım ile aynı ülkeye sahip farklı bir takım var! Seçilen Takım : " + selecteDTeam.TeamName);
                    pocketId--;
                }
            }
            if (selectedTeamsByPocketId.Count < 1)
            {
                btnPullOut.Enabled = false;
                CreateFixture();
            }

            RefreshDataGrids();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            pocketId = 0;
            groupId = 64;
            var allTeams = teams.GetAll().ToList();
            DeleteFixture();
            DeletePointStates();
            foreach (var team in allTeams)
            {
                team.GroupId = ' ';
                teams.Update(team);
            }
            RefreshDataGrids();
            btnPullOut.Enabled = true;
            btnPlay.Enabled = true;
            btnOtoPlay.Enabled = true;
        }

        private void DeletePointStates()
        {
            var data = pointStateService.GetAll();
            foreach (var del in data)
            {
                del.Drawn = 0; del.GoalAgainst = 0;
                del.GoalDifference = 0; del.Played = 0;
                del.Lost = 0; del.GoalFor = 0;
                del.Won = 0; del.Points = 0;
                pointStateService.Update(del);
            }
            GetPointStates();
        }

        private void DeleteFixture()
        {
            var data = matchService.GetAll();
            foreach (var del in data)
            {
                matchService.Delete(del);
            }
            cbBoxTeam.Items.Clear();
            GetFixture();
        }

        private void Oto_Click(object sender, EventArgs e)
        {
            btnRestart_Click(sender, e);
            var errorCount = 0;
            do
            {
                if (errorCount > 4) { errorCount = 0; Oto_Click(sender, e); }
                if (pocketId == 4) pocketId = 1; else pocketId++;
                var allTeamsByGroupId = teams.GetAll().Where(g => g.GroupId == ' ').ToList();
                var selectedTeamsByPocketId = allTeamsByGroupId.Where(t => t.PocketId == pocketId).ToList();
                if (selectedTeamsByPocketId.Count < 1)
                {
                    btnPullOut.Enabled = false;
                }
                else
                {
                    var selecteDTeam = selectedTeamsByPocketId[random.Next(selectedTeamsByPocketId.Count)];
                    selecteDTeam.GroupId = (allTeamsByGroupId.Count % 4 == 0) ? (char)(++groupId) : (char)groupId;

                    if ((teams.GetAll().FirstOrDefault(c => c.CountryId == selecteDTeam.CountryId & c.GroupId == selecteDTeam.GroupId) is null))
                    {
                        teams.Update(selecteDTeam);
                        errorCount = 0;
                    }
                    else
                    {
                        pocketId--;
                        errorCount++;
                    }
                }


            } while (teams.GetAll().Where(g => g.GroupId == ' ').ToList().Count > 0);
            RefreshDataGrids();
            CreateFixture();
            btnPullOut.Enabled = false;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
        }

        private void GetPointStates()
        {
            dgvPointStates.DataSource = "";
            filteredData.Clear();
            var data = pointStateService.GetPointStateDetails();

            foreach (var item in grpBoxPointState.Controls)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    CheckBox chk = (CheckBox)item;
                    if (chk.Checked && !(chk.Name.EndsWith("All")))
                    {
                        filteredData.AddRange(data.Where(g => g.GroupId == chk.Name.Last()));
                    }
                }
            }
            dgvPointStates.DataSource = filteredData.OrderBy(g => g.GroupId).ThenByDescending(p => p.Points).ThenByDescending(gd => gd.GoalDifference).ThenByDescending(gf=>gf.GoalFor).ToList();
            dgvPointStates.Columns[0].Visible = false;
            dgvPointStates.Columns["GroupId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvPointStates.Columns["GroupId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPointStates.Columns["GroupId"].HeaderText = "Group";
            dgvPointStates.Columns["TeamName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvPointStates.Columns["TeamName"].HeaderText = "Club";
            dgvPointStates.Columns["GoalFor"].HeaderText = "GF";
            dgvPointStates.Columns["GoalAgainst"].HeaderText = "GA";
            dgvPointStates.Columns["GoalDifference"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPointStates.Columns["GoalDifference"].HeaderText = "GD";
            dgvPointStates.Columns["Played"].HeaderText = "PL";
            dgvPointStates.Columns["Won"].HeaderText = "W";
            dgvPointStates.Columns["Drawn"].HeaderText = "D";
            dgvPointStates.Columns["Lost"].HeaderText = "L";
            dgvPointStates.Columns["Points"].HeaderText = "P";

            for (int i = 0; i < dgvPointStates.RowCount; i++)
            {
                if (i % 4 == 0 || i % 4 == 1)
                {
                    dgvPointStates.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    dgvPointStates.Rows[i].DefaultCellStyle.BackColor = Color.PaleVioletRed;
                }
            }

            dgvPointStates.Refresh();
        }

        private void tabGroupTeams_Selected(object sender, TabControlEventArgs e)
        {
            if (tabGroupTeams.SelectedIndex == 1)
            {
                GetTeams();
                GetFixture();
                GetPointStates();
            }
        }

        private void GetTeams()
        {
            cbBoxTeam.Items.Clear();
            cbBoxTeam.Items.Add("Choose a Club...");
            var data = teams.GetAll().OrderBy(t => t.TeamName).ToList();
            foreach (var team in data)
            {
                cbBoxTeam.Items.Add(team.TeamName);
            }
            cbBoxTeam.SelectedIndex = 0;

        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
            {
                foreach (var item in grpBoxPointState.Controls)
                {
                    if (item.GetType() == typeof(CheckBox))
                    {

                        CheckBox chk = (CheckBox)item;

                        if (chk.Name.EndsWith("All"))
                            chk.Checked = false;
                        else chk.Checked = true;
                    }
                }
            }
            else
            {

            }

        }

        private void checkBoxA_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();

        }

        void AllUnChecked()
        {
            checkBoxAll.Checked = false;
        }

        private void checkBoxB_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();
        }

        private void checkBoxC_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();
        }

        private void checkBoxE_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();
        }

        private void checkBoxD_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();
        }

        private void checkBoxF_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();
        }

        private void checkBoxG_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();
        }

        private void checkBoxH_CheckedChanged(object sender, EventArgs e)
        {
            AllUnChecked();
            GetPointStates();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            var data = matchService.GetAll().Where(p => p.Played == false).ToList(); //oynanmamış maçları dataya aktar
            if (data.Count == 0)
            {
                btnPlay.Enabled = false;
                btnOtoPlay.Enabled = false;
                return;
            }
            var goalHome = (byte)random.Next(9); //evsahibi random gol
            var goalAway = (byte)random.Next(9); //deplasman random gol
            var selectedData = data[random.Next(data.Count)]; //datadan tasrgele maç seç
            selectedData.Played = true;      //maç oynandı işaretle
            selectedData.HomeGoals = goalHome;//atılan gol
            selectedData.AwayGoals = goalAway;//yenilen gol
            var pointStates = pointStateService.GetAll(); //puan durumlarını pointStates'a aktar
            var HomePointStates = pointStates.Where(x => x.TeamId == selectedData.HomeId).First(); //seçilen maçın ev sahibi puan durumu
            var AwayPointStates = pointStates.Where(x => x.TeamId == selectedData.AwayId).First(); // seçilen maçın deplasman puan durumu
            HomePointStates.Played++; AwayPointStates.Played++; //oynanan maç toplamları 1 arttır.
            HomePointStates.GoalFor += goalHome; AwayPointStates.GoalFor += goalAway; //atılan gol top. up
            HomePointStates.GoalAgainst += goalAway; AwayPointStates.GoalAgainst += goalHome; //yenilen gol top. up
            HomePointStates.GoalDifference = (sbyte)((int)HomePointStates.GoalFor - (int)HomePointStates.GoalAgainst);
            AwayPointStates.GoalDifference = (sbyte)((int)AwayPointStates.GoalFor - (int)AwayPointStates.GoalAgainst);

            if (goalHome > goalAway) //evsahibi kazandıysa
            {
                HomePointStates.Won++;
                AwayPointStates.Lost++;
                HomePointStates.Points += 3;
            }
            else if (goalHome < goalAway)
            {
                HomePointStates.Lost++;
                AwayPointStates.Won++;
                AwayPointStates.Points += 3;
            }
            else
            {
                HomePointStates.Drawn++;
                AwayPointStates.Points++;
                HomePointStates.Points++;
            }

            pointStateService.Update(HomePointStates);
            pointStateService.Update(AwayPointStates);
            matchService.Update(selectedData);
            GetFixture();
            GetPointStates();

        }

        private void btnOtoPlay_Click(object sender, EventArgs e)
        {
            bool data = true;
            while (data)
            {
                btnPlay_Click(sender, e);
                data = matchService.GetAll().Where(p => p.Played == false).Any();
            }
            if (!data)
            {
                btnOtoPlay.Enabled = false;
                btnPlay.Enabled = false;
                MessageBox.Show("All Teams Played!");
            }
        }

        void GetFixture(string team = null)
        {
            dgvFixture.DataSource = "";
            var data = matchService.GetMatchDetails().ToList();
            if (team is null)
                dgvFixture.DataSource = data.OrderByDescending(h => h.Played).ToList();
            else
                dgvFixture.DataSource = data.Where(t => t.HomeTeamName == team || t.AwayTeamName == team)
                    .OrderByDescending(h => h.Played).ToList();

            dgvFixture.Columns["Id"].Visible = false;
            dgvFixture.Columns["HomeTeamName"].DisplayIndex = 0;
            dgvFixture.Columns["HomeTeamName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFixture.Columns["HomeTeamName"].HeaderText = "Home";
            dgvFixture.Columns["HomeGoals"].DisplayIndex = 1;
            dgvFixture.Columns["HomeGoals"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFixture.Columns["HomeGoals"].HeaderText = "Goals";
            dgvFixture.Columns["AwayGoals"].DisplayIndex = 2;
            dgvFixture.Columns["AwayGoals"].HeaderText = "Goals";
            dgvFixture.Columns["AwayTeamName"].DisplayIndex = 3;
            dgvFixture.Columns["AwayTeamName"].HeaderText = "Away";
            dgvFixture.Columns["Played"].Visible = false;

            for (int i = 0; i < dgvFixture.RowCount; i++)
            {
                if((bool)(dgvFixture.Rows[i].Cells[5].Value))
                {
                    if((byte)(dgvFixture.Rows[i].Cells[3].Value)> (byte)(dgvFixture.Rows[i].Cells[4].Value)){
                        dgvFixture.Rows[i].Cells[1].Style.BackColor = Color.LightGreen;
                        dgvFixture.Rows[i].Cells[3].Style.BackColor = Color.LightGreen;
                        dgvFixture.Rows[i].Cells[2].Style.BackColor = Color.PaleVioletRed;
                        dgvFixture.Rows[i].Cells[4].Style.BackColor = Color.PaleVioletRed;
                    }
                    else if((byte)(dgvFixture.Rows[i].Cells[3].Value) < (byte)(dgvFixture.Rows[i].Cells[4].Value))
                    {
                        dgvFixture.Rows[i].Cells[2].Style.BackColor = Color.LightGreen;
                        dgvFixture.Rows[i].Cells[4].Style.BackColor = Color.LightGreen;
                        dgvFixture.Rows[i].Cells[1].Style.BackColor = Color.PaleVioletRed;
                        dgvFixture.Rows[i].Cells[3].Style.BackColor = Color.PaleVioletRed;
                    }
                    else
                    {
                        dgvFixture.Rows[i].Cells[1].Style.BackColor = Color.LightGray;
                        dgvFixture.Rows[i].Cells[2].Style.BackColor = Color.LightGray;
                        dgvFixture.Rows[i].Cells[3].Style.BackColor = Color.LightGray;
                        dgvFixture.Rows[i].Cells[4].Style.BackColor = Color.LightGray;
                    }
                }
            }

            dgvFixture.Refresh();
        }

        void CreateFixture()
        {
            DeleteFixture();
            List<Team> data = teams.GetAll().ToList();
            List<Team> orderedData = data.OrderBy(x => x.GroupId).ToList();

            while (orderedData.Any()) //Datata eleman olduğu sürece
            {
                var newList = orderedData.Take(4).ToList(); //newListeye datanın ilk 4 elemanını al
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (newList[i].Id != newList[j].Id)
                        {
                            matchService.Add(new Match
                            {
                                AwayGoals = 0,
                                AwayId = newList[j].Id,
                                HomeGoals = 0,
                                HomeId = newList[i].Id,
                            });
                        }
                    }
                }

                orderedData = orderedData.Skip(4).ToList(); //datanın ilk 4 elemanını yoksayıp datayı tekrar dataya ata
            }
        }

        private void cbBoxTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetFixture(cbBoxTeam.SelectedText);
        }

        private void cbBoxTeam_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbBoxTeam.SelectedIndex == 0)
            {
                GetFixture();
            }
            else
                GetFixture(cbBoxTeam.GetItemText(cbBoxTeam.SelectedItem));
        }
    }
}
