﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CardInfoDTO
    {
        CardDTO card;
        String cardName;
        List<string> listNameUser = new List<string>();
        List<ChecklistDTO> listChecklist = new List<ChecklistDTO>();
        List<ChecklistDTO> listCheckedlist = new List<ChecklistDTO>();
        List<CommentDTO> commentDTOs = new List<CommentDTO>();
        CardUserDTO user;

        public CardInfoDTO(CardDTO card, List<String> listNameUser, List<ChecklistDTO> listChecklist,
                           List<ChecklistDTO> listCheckedlist, CardUserDTO user, List<CommentDTO> commentDTOs)
        {
            this.card = card;
            this.listNameUser = listNameUser;
            this.listChecklist = listChecklist;
            this.listCheckedlist = listCheckedlist;
            this.user = user;
            this.commentDTOs = commentDTOs;
        }

        public CardDTO Card { get => card; set => card = value; }
        public List<string> ListNameUser { get => listNameUser; set => listNameUser = value; }
        public List<ChecklistDTO> ListChecklist { get => listChecklist; set => listChecklist = value; }
        public List<ChecklistDTO> ListCheckedlist { get => listCheckedlist; set => listCheckedlist = value; }
        public CardUserDTO User { get => user; set => user = value; }
        public List<CommentDTO> CommentDTOs { get => commentDTOs; set => commentDTOs = value; }
    }
}
