using SgkViziteService.Models;
using System.ServiceModel;
using ViziteGonderService;

namespace SgkViziteService.Services
{
    public class SgkService
    {

        public EndpointAddress endpointAddress;
        public BasicHttpBinding basicHttpBinding;
        private readonly ViziteGonderService.ViziteGonderClient _service;
        private string Token { get; set; }
        public string IsyeriKodu { get; set; }
        public string IsyeriSifresi { get; set; }
        public string KullaniciAdi { get; set; }
        public SgkService(string url, string kullaniciAdi, string isyeriKodu, string isyeriSifresi)
        {
            endpointAddress = new EndpointAddress(url);

            basicHttpBinding = new BasicHttpBinding(endpointAddress.Uri.Scheme.ToLower() == "http"
                ? BasicHttpSecurityMode.None
                : BasicHttpSecurityMode.Transport);
            basicHttpBinding.OpenTimeout = TimeSpan.MaxValue;
            basicHttpBinding.CloseTimeout = TimeSpan.MaxValue;
            basicHttpBinding.ReceiveTimeout = TimeSpan.MaxValue;
            basicHttpBinding.SendTimeout = TimeSpan.MaxValue;
            basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;
            _service = new ViziteGonderService.ViziteGonderClient(basicHttpBinding, endpointAddress);
            KullaniciAdi = kullaniciAdi;
            IsyeriKodu = isyeriKodu;
            IsyeriSifresi = isyeriSifresi;
        }

        public async Task<ActionResponse<string>> LogiAsyncn() 
        {

            var response = ActionResponse<string>.Success(200);

            var result = await _service.wsLoginAsync(new ViziteGonderService.wsLoginRequest
            {
                isyeriKodu = IsyeriKodu,
                isyeriSifresi = IsyeriSifresi,
                kullaniciAdi = KullaniciAdi
            });
            if (result.wsLoginReturn.sonucKod !=0)
            {
                response.Message = result.wsLoginReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
             
            }
            Token = response.Data = result.wsLoginReturn.wsToken;
            return response;
        }


        public async Task<ActionResponse<string>> RaporAramaTarihileAsync(string tarih)
        {
            var response = await LogiAsyncn();
            if (response.ResponseType == ResponseType.Error)
            {
                return response;
            }

            var result = await _service.raporAramaTarihileAsync(new raporAramaTarihileRequest
            {
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                tarih = tarih
            });
            if (result.raporAramaTarihileReturn.sonucKod != 0)
            {
                
                response.Message = result.raporAramaTarihileReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            return response;
        }

        public async Task<ActionResponse<string>> IsverenIletisimBilgileriGoruntuAsync()
        {
            var response = await LogiAsyncn();
            if (response.ResponseType == ResponseType.Error)
            {
                return response;
            }

            var result = await _service.isverenIletisimBilgileriGoruntuAsync(new ViziteGonderService.isverenIletisimBilgileriGoruntuRequest
            {
                wsToken = Token,
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi
            });

            if (result.isverenIletisimBilgileriGoruntuReturn.sonucKod != 0)
            {
                response.Message = result.isverenIletisimBilgileriGoruntuReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
            }
           
            return response;
        }

        public async Task<ActionResponse<string>> RaporAramaKimlikNoAsync(string tckNo)
        {
            var response = await LogiAsyncn();
            if (response.ResponseType == ResponseType.Error)
            {
                return response;
            }
            var result = await _service.raporAramaKimlikNoAsync(new ViziteGonderService.raporAramaKimlikNoRequest
            {
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                tckNo = tckNo
            });
            if (result.raporAramaKimlikNoReturn.sonucKod != 0)
            {
                response.Message = result.raporAramaKimlikNoReturn.sonucAciklama;
                response.ResponseType= ResponseType.Error;
                return response;
            }
           
            return response;
        }


    }
}
