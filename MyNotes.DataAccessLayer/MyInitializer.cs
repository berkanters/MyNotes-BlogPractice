using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.EntityLayer;

namespace MyNotes.DataAccessLayer
{
    public class MyInitializer:CreateDatabaseIfNotExists<MyNotesContext>
    {
        protected override void Seed(MyNotesContext context)
        {
            MyNotesUser admin = new MyNotesUser()
            {
                Name = "Berkant",
                LastName = "Ersoy",
                Email = "berkantersoy1995@gmail.com",
                //ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                UserName = "BerkantE",
                Password = "12345",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUserName = "system"

            };
            MyNotesUser stdUser = new MyNotesUser()
            {
                Name = "Recep",
                LastName = "Ivedik",
                Email = "reco3434@gmail.com",
                //ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "Reco43",
                Password = "54321",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUserName = "system"

            };
            context.MyNotesUsers.Add(admin);
            context.MyNotesUsers.Add(stdUser);
            for (int i = 0; i < 8; i++)
            {
                MyNotesUser user = new MyNotesUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    LastName = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    //ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user-{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = $"user-{i}"
                };
                context.MyNotesUsers.Add(user);
            }
            context.SaveChanges();
            List<MyNotesUser> userList = context.MyNotesUsers.ToList();

            for (int i = 0; i < 10; i++)
            {
                //adding Categories
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = "system"
                };
                context.Catagories.Add(cat);
                //Adding Notes
                for (int j = 0; j < FakeData.NumberData.GetNumber(5,9); j++)
                {
                    MyNotesUser owner = userList[FakeData.NumberData.GetNumber(1, userList.Count)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5 ,25)),
                        text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1,9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUserName = owner.UserName
                    };
                    cat.Notes.Add(note);
                    //Adding Comment
                    for (int k = 0; k < FakeData.NumberData.GetNumber(3,5); k++)
                    {
                        MyNotesUser comment_owner = userList[FakeData.NumberData.GetNumber(1, userList.Count)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUserName = comment_owner.UserName
                        };
                        note.Comments.Add(comment);

                    }
                    //Adding Liked
                    for (int k = 0; k < note.LikeCount; k++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[k],
                        };
                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
