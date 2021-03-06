﻿using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class CardDetail : Form
    {
        int _cardId;
        ListSpace _listSpace;

        CardDTO cardDTO;
        ListDTO listDTO;
        ChecklistDTO checklistDTO;
        CommentDTO commentDTO;

        CardBLL cardBLL = new CardBLL();
        ListBLL listBLL = new ListBLL();
        ChecklistBLL checklistBLL = new ChecklistBLL();
        UserBLL userBLL = new UserBLL();
        CommentBLL commentBLL = new CommentBLL();
        ActivityBLL activityBLL;

        List<ChecklistDTO> checklistDTOs;
        List<CheckBox> tasks = new List<CheckBox>();
        List<String> listNameUser = new List<string>();
        List<CommentDTO> commentDTOs;

        public CardDetail(int id, ListSpace listSpace)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _cardId = id;
            _listSpace = listSpace;
            cardDTO = cardBLL.GetCard(_cardId);

            switch (cardDTO.Label)
            {
                case 1:
                    this.cardLabel.BackColor = Color.Red;
                    break;
                case 2:
                    this.cardLabel.BackColor = Color.Yellow;
                    break;
                case 3:
                    this.cardLabel.BackColor = Color.Green;
                    break;
                case 4:
                    this.cardLabel.BackColor = Color.Orange;
                    break;
                case 5:
                    this.cardLabel.BackColor = Color.Blue;
                    break;
                case 6:
                    this.cardLabel.BackColor = Color.Fuchsia;
                    break;
                default:
                    this.cardLabel.BackColor = Color.Transparent;
                    break;
            }
            this.CardName.Text = cardDTO.Title;
            this.descriptionText.Text = cardDTO.Description;
            this.checkDueDate.Text = cardDTO.DueDate.Date.ToString();

            AddMember();
            

            this.descriptionText.LostFocus += DescriptionText_LostFocus;
            this.commentText.LostFocus += CommentText_LostFocus;

            listDTO = listBLL.GetList(cardDTO.ListId);
            this.List.Text = listDTO.Title;
            //_boardId = listDTO.BoardId;

            commentDTOs = commentBLL.GetAllComments(_cardId);
            foreach (CommentDTO comment in commentDTOs)
            {
                UserComment userComment = new UserComment(userBLL.GetUser(comment.UserId).Name.Substring(0, 1), comment.Content);
                cmtPanel.Controls.Add(userComment);
            }

            activityPanel.Controls.Clear();
        }

        private void AddMember()
        {
            this.memberFlp.Controls.Clear();
            listNameUser = userBLL.GetListNameUserWorkingFor(_cardId);

            foreach (String name in listNameUser)
            {
                MemIcon member = new MemIcon(name);
                this.memberFlp.Controls.Add(member);
            }
        }

        private void CommentText_LostFocus(object sender, EventArgs e)
        {
            if (commentText.Text == "")
            {
                commentText.Text = "Thêm bình luận...";
            }
        }

        private void DescriptionText_LostFocus(object sender, EventArgs e)
        {
            if (descriptionText.Text == "")
            {
                descriptionText.Text = "Thêm mô tả...";
            }
        }


        private void AddMem_Click(object sender, EventArgs e)
        {
            MemberEdit editMember = new MemberEdit(this.Location.X + this.Width, this.Location.Y, _cardId, Global.id_Board);
            editMember.Show();
        }

        private void EditLabel_Click(object sender, EventArgs e)
        {
            LabelEdit labelEdit = new LabelEdit(this.Location.X + this.Width, this.Location.Y + EditLabel.Location.Y, _cardId);
            labelEdit.Show();
        }

        private void DueDate_Click(object sender, EventArgs e)
        {
            DateEdit dateEdit = new DateEdit(this.Location.X + this.Width, this.Location.Y + DueDate.Location.Y, _cardId);
            dateEdit.Show();
        }

        private void moveBtn_Click(object sender, EventArgs e)
        {
            MoveForm moveForm = new MoveForm(this.Location.X + this.Width, this.Location.Y + moveBtn.Location.Y, _cardId);
            moveForm.Show();
        }

        private void CardName_MouseEnter(object sender, EventArgs e)
        {
            CardName.BackColor = Color.White;
            CardName.BorderStyle = BorderStyle.FixedSingle;
        }

        private void CardName_MouseLeave(object sender, EventArgs e)
        {
            CardName.BackColor = Color.WhiteSmoke;
            CardName.BorderStyle = BorderStyle.None;
        }

        private void descriptionText_MouseClick(object sender, MouseEventArgs e)
        {
            if (descriptionText.Text == "Thêm mô tả...")
            {
                descriptionText.Text = "";
            }
        }

        private void commentText_MouseClick(object sender, MouseEventArgs e)
        {
            if (commentText.Text == "Thêm bình luận...")
            {
                commentText.Text = "";
            }
        }

        private void CloseButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void ChecklistBtn_Click(object sender, EventArgs e)
        {
            checklistPn.Visible = true;
            progressBar1.Visible = true;
            //taskFlpanel.Controls.Clear();
            //checklistDTOs = checklistBLL.GetAllChecklist(_cardId);
            //foreach (ChecklistDTO checklist in checklistDTOs)
            //{
            //    CheckBox task = new CheckBox();
            //    task.Font = new Font(task.Font.FontFamily, 10.0f);
            //    task.Text = checklist.Title;
            //    if (checklist.Status == 1)
            //    {
            //        task.Checked = true;
            //    }
            //    else task.Checked = false;
            //    taskFlpanel.Controls.Add(task);
            //    tasks.Add(task);
            //}
        }

        private void addTask_Click(object sender, EventArgs e)
        {
            ChecklistEdit checklist = new ChecklistEdit(this.Location.X + this.Width, this.Location.Y + ChecklistBtn.Location.Y, _cardId);
            checklist.Show();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            checklistBLL.DeleteChecklist(_cardId);
            taskFlpanel.Visible = false;
            tasks.Clear();
        }
        
        private void SaveButton_Click(object sender, EventArgs e)
        {
            cardDTO.Title = this.CardName.Text;
            cardDTO.Description = descriptionText.Text;

            progressBar1.Value = 0;
            progressBar1.Maximum = checklistDTOs.Count();
            foreach (CheckBox task in tasks)
            {
                if (task.Checked == true)
                {
                    progressBar1.Increment(1);
                    checklistDTO = checklistBLL.GetChecklist(task.Text);
                    checklistDTO.Status = 1;
                    checklistBLL.UpdateChecklist(checklistDTO);
                }
                else
                {
                    checklistDTO = checklistBLL.GetChecklist(task.Text);
                    checklistDTO.Status = 0;
                    checklistBLL.UpdateChecklist(checklistDTO);
                }
            }

            if (followCheck.Checked == true)
            {
                followPic.Visible = true;
            }
            else followPic.Visible = false;
            if (checklistDTOs.Count() != 0)
            {
                checklistPn.Visible = true;
                progressBar1.Visible = true;
            }
            else checklistPn.Visible = false;

            cardBLL.UpdateCard(cardDTO);
        }

        private void commentButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(commentText.Text))
            {
                activityBLL = new ActivityBLL();
                commentDTO = new CommentDTO(_cardId, Global.user.UserId, commentText.Text, DateTime.Now, 1);

                try
                {
                    commentBLL.InsertComment(commentDTO);
                }
                catch { MessageBox.Show("Mỗi user chỉ comment 1 lần", "Waring!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                UserComment userComment = new UserComment(userBLL.GetUser(Global.user.UserId).Name.Substring(0, 1), commentText.Text);
                cmtPanel.Controls.Add(userComment);

                activityBLL.InsertActivity(Global.user.UserId, Global.id_Board, Global.user.Name + " Has comment to card " + cardDTO.Title, DateTime.Now);
            }
        }

        private void CardDetail_Activated(object sender, EventArgs e)
        {
            cardDTO = cardBLL.GetCard(_cardId);

            AddMember();

            checkDueDate.Text = cardDTO.DueDate.ToString();

            switch (cardDTO.Label)
            {
                case 1:
                    this.cardLabel.BackColor = Color.Red;
                    break;
                case 2:
                    this.cardLabel.BackColor = Color.Yellow;
                    break;
                case 3:
                    this.cardLabel.BackColor = Color.Green;
                    break;
                case 4:
                    this.cardLabel.BackColor = Color.Orange;
                    break;
                case 5:
                    this.cardLabel.BackColor = Color.Blue;
                    break;
                case 6:
                    this.cardLabel.BackColor = Color.Fuchsia;
                    break;
                default:
                    this.cardLabel.BackColor = Color.Transparent;
                    break;
            }

            listDTO = listBLL.GetList(cardDTO.ListId);
            List.Text = listDTO.Title;

            taskFlpanel.Controls.Clear();
            tasks.Clear();
            checklistDTOs = checklistBLL.GetAllChecklist(_cardId);
            if (checklistDTOs.Count() != 0)
            {
                this.checklistPn.Visible = true;
                this.progressBar1.Visible = true;
                this.progressBar1.Maximum = checklistDTOs.Count();
                taskFlpanel.Controls.Clear();
                checklistDTOs = checklistBLL.GetAllChecklist(_cardId);
                foreach (ChecklistDTO checklist in checklistDTOs)
                {
                    CheckBox task = new CheckBox();
                    task.Font = new Font(task.Font.FontFamily, 10.0f);
                    task.Text = checklist.Title;
                    if (checklist.Status == 1)
                    {
                        task.Checked = true;
                        this.progressBar1.Increment(1);
                    }
                    else task.Checked = false;
                    taskFlpanel.Controls.Add(task);
                    tasks.Add(task);
                }
            }
            else checklistPn.Visible = false;
            progressBar1.Value = 0;
            progressBar1.Maximum = checklistDTOs.Count();
            foreach (CheckBox task in tasks)
            {
                if (task.Checked == true)
                {
                    progressBar1.Increment(1);
                    checklistDTO = checklistBLL.GetChecklist(task.Text);
                    checklistDTO.Status = 1;
                    checklistBLL.UpdateChecklist(checklistDTO);
                }
                else
                {
                    checklistDTO = checklistBLL.GetChecklist(task.Text);
                    checklistDTO.Status = 0;
                    checklistBLL.UpdateChecklist(checklistDTO);
                }
            }
        }

        private void CardDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            _listSpace.LoadListOfThisBoard();
            GC.Collect();
        }
    }
}
