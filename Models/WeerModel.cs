namespace KorteBroeken.Models
{
    public class WeerModel
    {
        public int Temperatuur { get; set; }
        public int Regenkans { get; set; }
        public bool KanKorteBroekAan { get; set; }
        public string Bericht { get; set; } = string.Empty;
    }
}