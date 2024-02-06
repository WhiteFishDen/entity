using System;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Threading.Tasks;

namespace entity
{
    public partial class Form1: Form
    {
        public class LibraryContext<T> : DbContext where T: class
        {
            private static string connString = "Host=localhost;Username=postgres;Password=123;Database=MusicShop;";
            
            public LibraryContext() : base(new NpgsqlConnection(connString), true) {  }
            public DbSet<T> product { get; set; } 
        }
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new string[] { "Music Record", "Performer", "Record Studio" });
            
            var db = new LibraryContext<MusicRecord>();
            db.product.Load();
            dataGridView1.DataSource = db.product.Local.ToBindingList();
            this.dataGridView1.Columns["Performer"].Visible = false;
            this.dataGridView1.Columns["RecordStudio"].Visible = false;

        }
        private void LoadTable()
        {
            var db = new LibraryContext<MusicRecord>();
            db.product.Load();
            dataGridView1.DataSource = db.product.Local.ToBindingList();
            this.dataGridView1.Columns["Performer"].Visible = false;
            this.dataGridView1.Columns["RecordStudio"].Visible = false;
        }
        

        [Table("record_studio", Schema = "public")]
        public class RecordStudio
        {
            [Column("id")] public int? Id { get; set; }
            [Column("name")] public string Name { get; set; }
            [Column("adress")] public string Adress { get; set; }

        }

        [Table ("performer", Schema = "public")]
        public class Performer
        {
            [Column("id")] public int? Id { get; set; }
            [Column("name")] public string Name { get; set; }
            [Column("year_of_foundation")] public DateTime? Year_of_foundation { get; set; }
        }



        [Table("music_record", Schema = "public")]
        public class MusicRecord
        {
            [Column("id")] public int? Id { get; set; }
            [Column("name")] public string Name { get; set; }
            [Column("year")] public DateTime? Date { get; set; }
            [Column("count_of_songs")] public int? Count_of_songs { get; set; }
            [Column("genre")] public string Genre { get; set; }
            [Column ("price")] public double? Price { get; set; }

            [Column("id_performer")] public int? PerformerId { get; set; }
            [Column("id_record_studio")] public int?  RecordStudioId { get; set; }

           public virtual  Performer Performer { get; set; }
           public virtual RecordStudio RecordStudio { get; set; }

        }
          


            void GetAllRecords()
            {
               using (LibraryContext<MusicRecord> context = new LibraryContext<MusicRecord>())
               {
                    var table = context.product.ToList();
                    foreach (var entity in table)
                    {
                    Invoke(new Action(() =>
                    { comboBox1.Items.Add(entity.Name + " " + entity.RecordStudio.Name + " " + entity.Performer.Name); })); 
                    }
               }
            }

        static void AddProduct(MusicRecord  prod)
        {
            using (LibraryContext<MusicRecord> ctx = new LibraryContext<MusicRecord>())
            {
                MusicRecord  a = ctx.product.Where((x) => x.Id == prod.Id).FirstOrDefault();
                if (a == null)
                {
                    ctx.product.Add(prod);
                    ctx.SaveChanges();
                }
            }
        }
        async Task PrintAsync()
        {
            await Task.Run(() => GetAllRecords());
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            await PrintAsync();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) {  }
        }



        //private async void button2_Click(object sender, EventArgs e)
        //{
        //    MusicRecord newRecord = new MusicRecord { Id = Convert.ToUInt16(textBox1.Text), 
        //                                              Name = textBox2.Text,
        //                                              Date = Convert.ToDateTime(textBox3.Text),
        //                                              Count_of_songs = };
        //}
    }
}
    

