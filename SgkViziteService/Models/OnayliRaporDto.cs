namespace SgkViziteService.Models;

public class OnayliRaporDto
{
    public string AD { get; set; }
    public string SOYAD { get; set; }
    public string VAKA { get; set; }
    public string VAKAADI { get; set; }
    public string TCKIMLIKNO { get; set; }
    public string RAPORSIRANO { get; set; }
    public string ISBASKONTTAR { get; set; }
    public string RAPORTAKIPNO { get; set; }
    public string MEDULARAPORID { get; set; }
    public string POLIKLINIKTAR { get; set; }
    public string ISKAZASITARIHI { get; set; }
}

public class OnayliRaporDetay
{
    public string BASTAR { get; set; }
    public string BITTAR { get; set; }
    public string BILDIRIM_ID { get; set; }
    public string ISLEM_TARIHI { get; set; }
    public string MEDULARAPOR_ID { get; set; }
    public string ODEMECIKTIMI { get; set; }
    public string CALISTI_CALISMADI { get; set; }
}