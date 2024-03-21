var mg = (function () {
    return {
        /**
         * Bir mesaj gösterilecek ise çağırılacak fonksiyondur.
         * @param {string} msg Gösterilecek mesaj
         */
        showMessage: function (msg) {
            swal(msg);
        },
        /**
         * Bir bilgi mesajı gösterilecek ise çağırılacak fonksiyondur. Başlık olarak "Bilgi" yazar.
         * @param {string} msg Gösterilecek mesaj
         */
        showInformationMessage: function (msg) {
            swal('Bilgi', msg, 'info');
        },
        /**
         * Başarılı bir işlem mesajı gösterilecek ise çağırılacak fonksiyondur. Başlık olarak "İşlem Başarılı" yazar
         * @param {any} msg Gösterilecek mesaj
         */
        showSuccessMessage: function (msg) {
            swal('İşlem Başarılı', msg, 'success');
        },
        /**
         * Başarısız bir işlem mesajı gösterilecek ise çağırılacak fonksiyondur. Başlık olarak "İşlem Başarısız" yazar
         * @param {any} msg Gösterilecek mesaj
         */
        showErrorMessage: function (msg) {
            swal('İşlem Başarısız', msg, 'error');
        },
        showProgress: function () {
            $("div#spinner").fadeIn("fast");
        },
        hideProgress: function () {
            $("div#spinner").fadeOut("fast");
        },
        showProgressTransparent: function () {
            $("div#spinnerTransparent").fadeIn("fast");
        },
        hideProgressTransparent: function () {
            $("div#spinnerTransparent").fadeOut("fast");
        },
        showLoader: function () {
            $('.page-loader-wrapper').fadeIn();
        },
        showOverlay: function (elm) {
            $("<div class='overlay'></div>").appendTo(elm);
        },
        hideOverlay: function (elm) {
            elm.find('.overlay').remove();
        },
        setDropDown: function (elem, data, value, text) {
            $(elem)
                .children()
                .remove()
                .end()
                .append('<option value="">Loading...</option>')
                .attr('disabled', true);

            let options = [];
            options.push('<option value="NULL">&lt;SEÇİNİZ&gt;</option>');
            $.each(data, function (index, item) {
                options.push('<option value="' + item[value] + '">' + item[text] + '</option>');
            });
            $(elem)
                .html(options.join(''))
                .attr('disabled', false);

        },
        postFormData: function (url, data, callback) {
            $.ajax({
                type: "POST",
                url: url,
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    callback(result);
                }
            });
        },

        postJsonData: function (url, data, callback) {
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (callback !== null)
                        callback(result);
                }
            });
        },

        getView: function (url, data, callback) {
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                success: function (result) {
                    callback(result);
                }
            });
        },
        //Commons
        /**
 * Bu fonksiyon db de kayıtlı bütün markaları belirtilen select elementine option olarak ekler.
 * 
 * @param elem - Eklelemenin yapılacağı select elementi.
 * @param selected - Seçilmesi istenilen değer, - Yok ise null gönderilmelidir.-
 * @param completed - İşlemler tamamlandıktan sonra çalıştırılmak istenilen fonksiyon, - Yok ise null gönderilmelidir.-
 */
        getBrands: function (elem, selected, completed) {
            $(elem)
                .children()
                .remove()
                .end()
                .append('<option value="">Seçiniz</option>')
                .attr('disabled', true);
            $.getJSON('/helper/getBrands', function (data) {
                let options = []
                $.each(data, function (index, item) {
                    options.push('<option value="' + item.brandId + '">' + item.brandName + '</option>');
                });
                $(elem)
                    .html(options.join(''))
                    .attr('disabled', false)
                    .val(selected);
                if (completed !== null)
                    completed();
            });
        },
        /**
         * Bu fonksiyon bir markaya ait modelleri bir select elementine eklemek için kullanılır.
         * @param {any} elem - Eklemenin yapılacağı select elementi.
         * @param {any} BrandId - Modelleri getirilecek markanın idsi.
         * @param {any} selected - Seçilmesi istenilen değer, - Yok ise null gönderilmelidir.-
         * @param {any} completed - İşlemler tamamlandıktan sonra çalıştırılmak istenilen fonksiyon, - Yok ise null gönderilmelidir.-
         */
        getModels: function (elem, BrandId, selected, completed) {
            $(elem)
                .children()
                .remove()
                .end()
                .append('<option value="">Seçiniz</option>')
                .attr('disabled', true);
            $.getJSON('/helper/getModels?BrandId=' + BrandId, function (data) {
                let options = []
                $.each(data, function (index, item) {
                    options.push('<option value="' + item.modelId + '">' + item.modelName + '</option>');
                });
                $(elem)
                    .html(options.join(''))
                    .attr('disabled', false)
                    .val(selected);
                if (completed !== null)
                    completed();
            });
        },
    };
}());