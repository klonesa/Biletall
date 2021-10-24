function Search() {

    var pnrNo = $('#pnrno').val();

    if (pnrNo == '')
        toastr.error("Lütfen aramak için numara giriniz!")

    else {

        $.ajax({
            type: 'GET',
            dataType: 'json',
            url: '/Pnr/Search',
            data: { pnrNo },
            success: function (data) {
                console.log(data);
                GeneralModalShow(data);
            }
        });
    }
}


function GeneralModalShow(data)
{
    var modalHeader = document.getElementById('modalHeader');
    var modalBody = document.getElementById('modalBody');

    modalHeader.innerHTML = "";
    modalHeader.innerHTML = "<h4><b>" + data.Nereden + "-" + data.Nereye + "</b>&nbsp;&nbsp&nbsp;&nbsp;" + data.SeyahatTarihi + "&nbsp;&nbsp&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;PNR: " + data.PnrNo + "</h4>";
    modalBody.innerHTML = "";
    modalBody.innerHTML =

        `
      <div class="col-md-12">
        <div class="row">
            <div class="col-md-3"><img src="https://biletall-cdn.mncdn.com/img-v7/logolar/otobus/`+ data.FirmaNo + `.png" width="150" height="70" /></div>
            <div class="col-md-3"><h5>`+ data.Nereden +`</h5></div>
            <div class="col-md-3"><h5>`+ data.YaklasikSeyehatSuresi + ` Saat</h5></div>
            <div class="col-md-3"><h5>`+ data.Nereye+`</h5></div>
        </div>
        <div class="col-md-12">Email: `+ data.Email + `&nbsp;&nbsp&nbsp;&nbsp; Telefon: ` + data.PhoneNumber +`</div>
        <div class="col-md-12">Otobüs tipi, güzergah ve seyehat süresi opsiyonel zorunşuluklardan dolayı değişebilir.</br>
        <span class="my-text">SAYIN YOLCULARIMIZ. İKİLİ KOLTUKLAR AİLELER İÇİNDİR.AİLE OLMADIĞINIZ HALDE BU KOLTUKLARI SATIN ALMANIZ DURUMUNDA YASAL SORUMLULUKLAR SİZE AİTDİR.</span>
        </br><h5 style="text-align:right;">TOPLAM ÜCRET: `+ data.Ucret +` TL</h5>
        <div class="col-md-12">Yolcu Bilgileri
            <table class="table table-hover">
              <thead>
                <tr>
                  <th scope="col">Yolcu Adı</th>
                  <th scope="col">Durum</th>
                  <th scope="col">Koltuk</th>
                  <th scope="col">E-Bilet No</th>
                  <th scope="col">Servis İsteği</th>
                  <th scope="col">Bilet İşlemlerim</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>`+data.FullName+`</td>
                  <td>`+ data.Durum+`</td>
                  <td>`+ data.KoltukNo+`</td>
                  <td>`+ data.EBiletNo+`</td>
                  <td>`+ data.ServisIstegi+`</td>
                  <td>`+ data.BiletIslemlerim +`</td>
                </tr>
              </tbody>
            </table>
        </div>
        </br>
        </br>
         <div class="col-md-12">Sefer Bilgileri
        <table class="table table-hover">
              <thead>
                <tr>
                  <th scope="col">Sefer Süresi</th>
                  <th scope="col">Sefer Tipi</th>
                  <th scope="col">Sefer No</th>
                  <th scope="col">Cinsiyet</th>
                  <th scope="col">Peron</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>`+ data.YaklasikSeyehatSuresi + `</td>
                  <td>`+ data.SeferTipi + `</td>
                  <td>`+ data.SeferNo + `</td>
                  <td>`+ data.Cinsiyet + `</td>
                  <td>`+ data.Peron + `</td>
                </tr>
              </tbody>
            </table>
        </div>
      </div>`;
    $('#GeneralModal').modal('show');
}