﻿using SgkViziteService.Models;
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
            if (result.wsLoginReturn.sonucKod != 0)
            {
                response.Message = result.wsLoginReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            Token = response.Data = result.wsLoginReturn.wsToken;
            return response;
        }


        public async Task<ActionResponse<List<RaporBeanDto>>> RaporAramaTarihileAsync(string tarih)
        {
            var response = ActionResponse<List<RaporBeanDto>>.Success(200);
            var loginResponse = await LogiAsyncn();
            if (loginResponse.ResponseType == ResponseType.Error)
            {
                response.Message = loginResponse.Message;
                response.ResponseType = ResponseType.Error;
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


            response.Data = result.raporAramaTarihileReturn.raporAramaTarihleBeanArray.Select(x => new RaporBeanDto
            {
                AD = x.AD,
                SOYAD = x.SOYAD,
                VAKA = x.VAKA,
                ARSIV = x.ARSIV,
                ABASTAR = x.ABASTAR,
                ABITTAR = x.ABITTAR,
                VAKAADI = x.VAKAADI,


                TCKIMLIKNO = x.TCKIMLIKNO,

                RAPORDURUMU = x.RAPORDURUMU,
                RAPORSIRANO = x.RAPORSIRANO,
                ISBASKONTTAR = x.ISBASKONTTAR,
                RAPORTAKIPNO = x.RAPORTAKIPNO,
                YATRAPBASTAR = x.YATRAPBASTAR,
                YATRAPBITTAR = x.YATRAPBITTAR,
                MEDULARAPORID = x.MEDULARAPORID,
                POLIKLINIKTAR = x.POLIKLINIKTAR,
                DOGUMONCBASTAR = x.DOGUMONCBASTAR,
                ISKAZASITARIHI = x.ISKAZASITARIHI,
            }).ToList();
            return response;
        }

        public async Task<ActionResponse<string>> IsverenIletisimBilgileriGoruntuAsync()
        {
            var response = await LogiAsyncn();
            if (response.ResponseType == ResponseType.Error)
            {
                return response;
            }

            var result = await _service.isverenIletisimBilgileriGoruntuAsync(
                new ViziteGonderService.isverenIletisimBilgileriGoruntuRequest
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

        public async Task<ActionResponse<List<RaporBeanDto>>> RaporAramaKimlikNoAsync(string tckNo)
        {
            var response = ActionResponse<List<RaporBeanDto>>.Success(200);
            var loginResponse = await LogiAsyncn();
            if (loginResponse.ResponseType == ResponseType.Error)
            {
                response.Message = loginResponse.Message;
                response.ResponseType = ResponseType.Error;
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
                response.ResponseType = ResponseType.Error;
                return response;
            }

            response.Data = result.raporAramaKimlikNoReturn.raporBeanArray.Select(x => new RaporBeanDto
            {
                AD = x.AD,
                SOYAD = x.SOYAD,
                VAKA = x.VAKA,
                ARSIV = x.ARSIV,
                ABASTAR = x.ABASTAR,
                ABITTAR = x.ABITTAR,
                VAKAADI = x.VAKAADI,
                TESISADI = x.TESISADI,
                TESISKODU = x.TESISKODU,
                TCKIMLIKNO = x.TCKIMLIKNO,
                RAPORBITTAR = x.RAPORBITTAR,
                RAPORDURUMU = x.RAPORDURUMU,
                RAPORSIRANO = x.RAPORSIRANO,
                ISBASKONTTAR = x.ISBASKONTTAR,
                RAPORTAKIPNO = x.RAPORTAKIPNO,
                YATRAPBASTAR = x.YATRAPBASTAR,
                YATRAPBITTAR = x.YATRAPBITTAR,
                MEDULARAPORID = x.MEDULARAPORID,
                POLIKLINIKTAR = x.POLIKLINIKTAR,
                DOGUMONCBASTAR = x.DOGUMONCBASTAR,
                ISKAZASITARIHI = x.ISKAZASITARIHI,
                BASHEKIMONAYTARIHI = x.BASHEKIMONAYTARIHI,
                ISVERENEBILDIRILDIGITARIH = x.ISVERENEBILDIRILDIGITARIH,
            }).ToList();
            return response;
        }

        public async Task<ActionResponse<string>> RaporOkunduKapatAsync(string medulaRaporId)
        {
            
            var response = await LogiAsyncn();
            if (response.ResponseType == ResponseType.Error)
            {
                return response;
            }

            var result = await _service.raporOkunduKapatAsync(new raporOkunduKapatRequest
            {
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                medulaRaporId = medulaRaporId
            });
            if (result.raporOkunduKapatReturn.sonucKod != 0)
            {
                response.Message = result.raporOkunduKapatReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            response.Data = result.raporOkunduKapatReturn.sonucAciklama;


            return response;
        }

        public async Task<ActionResponse<List<OnayliRaporDetay>>> OnayliRaporlarDetayAsync(string medulaRaporId)
        {
            var response = ActionResponse<List<OnayliRaporDetay>>.Success(200);

            var loginResponse = await LogiAsyncn();
            if (loginResponse.ResponseType == ResponseType.Error)
            {
                response.Message = loginResponse.Message;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            var result = await _service.onayliRaporlarDetayAsync(new onayliRaporlarDetayRequest
            {
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                medulaRaporId = medulaRaporId
            });
            if (result.onayliRaporlarDetayReturn.sonucKod != 0)
            {
                response.Message = result.onayliRaporlarDetayReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            response.Data = result.onayliRaporlarDetayReturn.onayliRaporDetayBean.Select(x => new OnayliRaporDetay
            {
                BASTAR = x.BASTAR,
                BITTAR = x.BITTAR,
                BILDIRIM_ID = x.BILDIRIM_ID,
                ISLEM_TARIHI = x.ISLEM_TARIHI,
                MEDULARAPOR_ID = x.MEDULARAPOR_ID,
                ODEMECIKTIMI = x.ODEMECIKTIMI,
                CALISTI_CALISMADI = x.CALISTI_CALISMADI
            }).ToList();
            
            
            return response;
        }

        public async Task<ActionResponse<string>> PersonelimDegildirAsync(string tckNo, string tarih, string vaka,
            string nitelikDurumu, string medulaRaporId)
        {
            var response = ActionResponse<string>.Success(200);

            var loginResponse = await LogiAsyncn();
            if (loginResponse.ResponseType == ResponseType.Error)
            {
                response.Message = loginResponse.Message;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            var result = await _service.personelimDegildirAsync(new personelimDegildirRequest
            {
                medulaRaporId = medulaRaporId,
                vaka = vaka,
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                tckNo = tckNo
            });
            if (result.personelimDegildirReturn.sonucKod != 0)
            {
                response.Message = result.personelimDegildirReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            response.Data = result.personelimDegildirReturn.sonucAciklama;

            return response;
        }

        public async Task<ActionResponse<string>> OnaylIptalAsync(string medulaRaporId, string bildirimId)
        {
            var response = ActionResponse<string>.Success(200);

            var loginResponse = await LogiAsyncn();
            if (loginResponse.ResponseType == ResponseType.Error)
            {
                response.Message = loginResponse.Message;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            var result = await _service.onaylIptalAsync(new onaylIptalRequest
            {
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                medulaRaporId = medulaRaporId,
                bildirimId = bildirimId
            });
            if (result.onaylIptalReturn.sonucKod != 0)
            {
                response.Message = result.onaylIptalReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            response.Data = result.onaylIptalReturn.sonucAciklama;

            return response;
        }

        public async Task<ActionResponse<List<OnayliRaporDto>>> OnayliRaporlarTarihileAsync(string tarih1, string tarih2)
        {
            var response = ActionResponse<List<OnayliRaporDto>>.Success(200);

            var loginResponse = await LogiAsyncn();
            if (loginResponse.ResponseType == ResponseType.Error)
            {
                response.Message = loginResponse.Message;
                response.ResponseType = ResponseType.Error;
                return response;
            }
            
            var result = await _service.onayliRaporlarTarihileAsync(new onayliRaporlarTarihileRequest
            {
                isyeriKodu = IsyeriKodu,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                tarih1 = tarih1,
                tarih2 = tarih2
            });

            if (result.onayliRaporlarTarihileReturn.sonucKod != 0)
            {
                response.Message = result.onayliRaporlarTarihileReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            response.Data = result.onayliRaporlarTarihileReturn.onayliRaporlarTarihleBeanArray.Select(x =>
                new OnayliRaporDto
                {
                    AD = x.AD,
                    SOYAD = x.SOYAD,
                    VAKA = x.VAKA,
                    VAKAADI = x.VAKAADI,
                    TCKIMLIKNO = x.TCKIMLIKNO,
                    RAPORSIRANO = x.RAPORSIRANO,
                    ISBASKONTTAR = x.ISBASKONTTAR,
                    RAPORTAKIPNO = x.RAPORTAKIPNO,
                    MEDULARAPORID = x.MEDULARAPORID,
                    POLIKLINIKTAR = x.POLIKLINIKTAR,
                    ISKAZASITARIHI = x.ISKAZASITARIHI
                }).ToList();

            return response;
        }

        public async Task<ActionResponse<string>> RaporOnayAsync(string tckNo, string tarih, string vaka,
            string nitelikDurumu, string medulaRaporId)
        {
            var response = ActionResponse<string>.Success(200);

            var loginResponse = await LogiAsyncn();
            if (loginResponse.ResponseType == ResponseType.Error)
            {
                response.Message = loginResponse.Message;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            var result = await _service.raporOnayAsync(new raporOnayRequest
            {
                tckNo = tckNo,
                kullaniciAdi = KullaniciAdi,
                wsToken = Token,
                isyeriKodu = IsyeriKodu,
                tarih = tarih,
                vaka = vaka,
                nitelikDurumu = nitelikDurumu,
                medulaRaporId = medulaRaporId
            });
            if (result.raporOnayReturn.sonucKod != 0)
            {
                response.Message = result.raporOnayReturn.sonucAciklama;
                response.ResponseType = ResponseType.Error;
                return response;
            }

            response.Data = result.raporOnayReturn.sonucAciklama;
            return response;
        }
    }
}