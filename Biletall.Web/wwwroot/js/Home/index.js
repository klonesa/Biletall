var gResult = null;
$(function () {
    jQuery('#datetimepicker').datetimepicker({
        format: 'Y/m/d',
        timepicker: false,
        minDate: 0
    });
    GetCityNereden();
    GetCityNereye();
});

function GetCityNereden() {
    $.ajax({
        type: 'GET',
        url: '/Home/CityList',
        success: function (data) {
            $('#neredenDropdown').select2({
                data: data,
                placeholder: "Şehir Seçiniz",
                width: '100%'
            });
        }
    });
}

function GetCityNereye() {
    $.ajax({
        type: 'GET',
        url: '/Home/CityList',
        success: function (data) {
            $('#nereyeDropdown').select2({
                data: data,
                placeholder: "Şehir Seçiniz",
                width: '100%'
            });
        }
    });
}

$('#neredenDropdown').on('select2:select', function (e) {
    var data = e.params.data.id;
    GetCityNereye();
    var nereyeDropdown = $("#nereyeDropdown");
    nereyeDropdown.find('option[value=' + data + ']').remove();
});

$('#nereyeDropdown').on('select2:select', function (e) {
    var data = e.params.data.id;
    GetCityNereden();
    var neredenDropdown = $("#neredenDropdown");
    neredenDropdown.find('option[value=' + data + ']').remove();
});

function SearchTicket() {

    var fromCity = $('#neredenDropdown').val();
    var toCity = $('#nereyeDropdown').val();
    var time = $('#datetimepicker').val();

    if (fromCity === '0' || toCity === '0') {
        toastr.error('Lütfen Şehir seçiniz')
    }
    else if (time === '')
        toastr.error('Lütfen tarih seçiniz')
    else {
        $.ajax({
            type: 'POST',
            url: '/Home/OtobusSefer',
            data: {
                nereden: fromCity,
                nereye: toCity,
                tarih: time
            },
            success: function (data) {
                gResult = null;
                gResult = data;
                Seferler(data);
            }
        });
    }
}

function Seferler(data) {
    var model = document.getElementById('seferList');
    var object = '';
    $.each(data, function (index, value) {
        object +=
            `<br/><br/>
            <div class="row">
            <div class="col-2">
                <img alt="`+ value.FirmaAdi + `" src="https://biletall-cdn.mncdn.com/img-v7/logolar/otobus/` + value.FirmaNo + `.png" width="150" height="70" />
            </div>
            <div class="col-2">
                <div>
                    <h3>`+ value.KalkisSaati + `</h3>
                </div>
                <div>
                    <h5><span class="entypo-back-in-time"></span>`+ value.YaklasikSeyahatSuresi + `</h5>
                </div>
            </div>
            <div class="col-2">
                <div>
                    <b>`+ value.KalkisNokta + `</b>
                    <i class="entypo-right-open-mini"></i>
                    <b>`+ value.VarisNokta + `</b>
                </div>
            </div>
            <div class="col-2">
                <div>`+ value.OtobusKoltukYerlesimTipi + ` <img src="https://biletall-cdn.mncdn.com/img-v7/ortak/ikonlar/bos_koltuk.svg" style="width:15px;">
                `+ OTipOzellik(value.OTipOzellik) + `</div>
            </div>
            <div class="col-2">
                <div class="price-sec">
                    <div class="seat-price">
                        <h3>`+ value.BiletFiyatiInternet + `<span> TL</span></h3>
                    </div>
                </div>
            </div>
            <div class="col-2">
                <div class="control-sec">
                    <a class="btn btn-danger" data-toggle="tooltip" data-placement="top" data-original-title="Koltuk Seç">Koltuk Seç</a>
                    <br/>
                    <a type="button" onclick="GeneralModalShow(`+ value.Id + `)" data-toggle="tooltip" data-placement="top" data-original-title="Detayları Göster">Detayları Göster</a>
                </div>
            </div>
        </div><br /><br />
`;

    });
    model.innerHTML = object;
}

function OTipOzellik(data) {
    var result = '';

    if (data[0] == '1')
        result += `<i class="fas fa-wifi" data-toggle="tooltip" data-placement="top" title="İnternet"></i>`;
    if (data[1] == '1')
        result += `<i class="fas fa-couch" data-toggle="tooltip" data-placement="top" title="Rahat Koltuk"></i>`
    //if (data[2] == '1')
    //    result +=`<i class="fas fa-tv" data-toggle="tooltip" data-placement="top" title="Koltuk Ekranı (Uydu Yayını)">`;
    //if (data[3] == '1')
    //    result +=`<i class="fas fa-toilet" data-toggle="tooltip" data-placement="top" title="WC">`;
    //if (data[4] == '1')
    //    result +=`<i class="fas fa-tv" data-toggle="tooltip" data-placement="top" title="TV (Genel)">`;
    //if (data[5] == '1')
    //    result +=`<i class="fas fa-satellite" data-toggle="tooltip" data-placement="top" title="Digiturk">`;
    //if (data[6] == '1')
    //    result +=`<i class="fas fa-headphones" data-toggle="tooltip" data-placement="top" title="Kulaklık">`;
    //if (data[7] == '1')
    //    result +=`<i class="fas fa-music" data-toggle="tooltip" data-placement="top" title="Müzik Yayını (Genel)">`;
    //if (data[8] == '1')
    //    result +=`<i class="fas fa-music" data-toggle="tooltip" data-placement="top" title="Müzik Yayını (Koltuk)">`;
    //if (data[9] == '1')
    //    result +=`<i class="fas fa-mobile" data-toggle="tooltip" data-placement="top" title="Cep Telefonu Serbest">`;
    //if (data[10] == '1')
    //    result +=`<i class="fas fa-plug" data-toggle="tooltip" data-placement="top" title="220 Volt Priz">`;
    //if (data[11] == '1')
    //    result +=`<i class="fas fa-tv" data-toggle="tooltip" data-placement="top" title="Koltuk Ekranı (MIT)">`;
    //if (data[12] == '1')
    //    result +=`<i class="fas fa-mosque" data-toggle="tooltip" data-placement="top" title=">Namaz Vakitlerinde Durur">`;
    //if (data[13] == '1')
    //    result +=`<i class="fas fa-satellite" data-toggle="tooltip" data-placement="top" title="LigTV">`;
    //if (data[14] == '1')
    //    result +=`<i class="fas fa-tv" data-toggle="tooltip" data-placement="top" title="Koltuk Ekranı (10 inç)">`;
    //if (data[15] == '1')
    //    result +=`<i class="fas fa-lightbulb" data-toggle="tooltip" data-placement="top" title="Okuma Lambası">`;
    //if (data[16] == '1')
    //    result +=`<i class="fas fa-radio" data-toggle="tooltip" data-placement="top" title="Radyo (Kişisel)">`;
    //if (data[17] == '1')
    //    result +=`<i class="fas fa-tv" data-toggle="tooltip" data-placement="top" title="Koltuk Ekranı (13 inç)">`;
    //if (data[18] == '1')
    //    result +=`<i class="fab fa-usb" data-toggle="tooltip" data-placement="top" title="USB Giriş">`;
    //if (data[19] == '1')
    //    result += `<i class="fas fa-hamburger" data-toggle="tooltip" data-placement="top" title="Kahvaltı">`;

    return result;
}

function GeneralModalShow(id) {
    var data = gResult.find(x => x.Id == id);
    var modalHeader = document.getElementById('modalHeader');
    var modalBody = document.getElementById('modalBody');

    modalHeader.innerHTML = "";
    modalHeader.innerHTML = "Gidiş Seferi Detayı";
    modalBody.innerHTML = "";
    modalBody.innerHTML =

        `<hr/>
    <div class="row">
        <div class="col-md-12">
           <div class="row">
                <div class="col-md-4 text-center">
                    <img src="https://biletall-cdn.mncdn.com/img-v7/logolar/otobus/`+ data.FirmaNo + `.png" width="150" height="70" />
                        <div class="my_text">
                            <b>`+ data.KalkisNokta + `</b>
                            <i class="entypo-right-open-mini"></i>
                            <b>`+ data.VarisNokta + `</b>
                        </div>
                </div>
                <div class="col-md-4 text-center">
                    <div>
                        <h3>`+ data.KalkisSaati + `</h3>
                    </div>
                </div>
                <div class="col-md-4 text-center">
                     <h4>`+ data.BiletFiyatiInternet + ` TL</h4>
                     <button type="button" class="btn btn-danger btn-sm">Satın Al</button>
                </div>
           </div>
        </div>
    </div>
<hr/>
<div class="row">
    <div class="col-md-12">
        <div class="row">
            <div class="col-md-6">
                <label><b>Seyahat Tarihi</b></label></br>
                <label>`+ data.SeyehatTarihi + `</label></br></br>
                <br/>
                <label><b>Sefer Süresi</b></label></br>
                <label>`+ data.SeyahatSuresi + ` sa</label></br></br>
            </div>
            <div class="col-md-6">
                <label><b>Sefer Tipi</b></label></br>
                <label>`+ data.SeferTipiAciklamasi + `</label></br></br>
                <br/>
                <label><b>Koltuk Tipi</b></label></br>
                <label>`+ data.OtobusKoltukYerlesimTipi + `</label></br></br>
            </div>
        </div>
    </div>
</div>
<hr/>
<div class="row">
    <div class="col-md-12"><label><b>Otobüsün Özellikleri</b></label></div></br>
    <div class="row">
        <div class="col-md-12">
            <div class="row">
                 `+ OTipOzellik(data.OTipOzellik) + `
            </div>
        </div>
    </div>
</div>
<hr/>
<div class="row">
    <div class="col-md-12"><label><b>`+ data.KalkisNokta + ` <i class="entypo-right-open-mini"></i> ` + data.VarisNokta + ` Güzergahı</b></label></div></br>
    <div class="row">
        <div class="col-md-12" id="guzergahlar">
            
        </div>
    </div>
</div>
`;
    var object2 = document.getElementById('guzergahlar');
    object2.appendChild(Guzergahlar(data.Guzergahlar));
    $('#GeneralModal').modal('show');
}

function Guzergahlar(data) {

    var mainDiv = document.createElement('div');
    mainDiv.setAttribute('class', 'row');
    var firstDiv = document.createElement('div');
    firstDiv.setAttribute('class', 'col-md-6');
    var secondDiv = document.createElement('div');
    secondDiv.setAttribute('class', 'col-md-6');
    var firstLabel = document.createElement('label');
    firstLabel.setAttribute('style', 'font-weight:bold')
    firstLabel.textContent = "Kalkış Noktası Adı";
    firstDiv.appendChild(firstLabel);
    var secondLabel = document.createElement('label');
    secondLabel.setAttribute('style', 'font-weight:bold')
    secondLabel.textContent = "Kalkış Saati";
    secondDiv.appendChild(secondLabel);

    $.each(data, function (index, value) {
        var labeKalkisYeri = document.createElement('label');
        labeKalkisYeri.textContent = value.KaraNoktaAd;
        firstDiv.appendChild(document.createElement("br"));
        firstDiv.appendChild(labeKalkisYeri);

        var labelKalkisSaat = document.createElement('label');
        labelKalkisSaat.textContent = value.KalkisTarihSaat;
        secondDiv.appendChild(document.createElement("br"));
        secondDiv.appendChild(labelKalkisSaat)

    });
    mainDiv.appendChild(firstDiv);
    mainDiv.appendChild(secondDiv);
    return mainDiv;
}

function OrderByData(saat, fiyat, onePlus, twoPlus) {
    var model = {
        Saat: saat,
        Fiyat: fiyat,
        OnePlus: onePlus,
        TwoPlus: twoPlus,
        Sefers: gResult
    };
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Home/OrderByData',
        data: model,
        success: function (data) {
            Seferler(data.Data);
            toastr.info("Sıralama yapılmıştır");
        }
    });
}