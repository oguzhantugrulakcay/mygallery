(function ($) {
    "use strict";

    $.fn.konseptSelector = function (options) {
        options = $.extend({}, $.fn.konseptSelector.defaultOptions, options);
        return this.each(function () {
            var search = '<span class="input-group-append"><button class="btn input-group-text btn-outline-secondary selector-search" type="button"><span class="fas fa-search"></span></button></span>';
            var times = '<span class="input-group-append"><button class="btn input-group-text selector-clear" type="button"><span class="fas fa-times"></span></button></span>';

            var el = $(this);
            el.wrap('<div class="input-group"></div>');
            el.after(search);
            el.data('ks', '');
            var konseptSelector = {
                settings: options,
                el: $(this),
                setValue: function (value, text) {
                    el.closest('.input-group').find('.input-group-append').remove();
                    el.after(times);
                    el.data('ks', value);
                    el.val(text)
                },
                getValue: function () {
                    return el.data('ks');
                },
                getText: function () {
                    return el.val();
                },
                clear: function () {
                    el.closest('.input-group').find('.input-group-append').remove();
                    el.after(search);
                    el.val('');
                    el.focus();
                    el.data('ks', '');
                }
            };

            el.parent('div').on("click", ".selector-clear", function (e) {
                e.preventDefault();
                konseptSelector.clear();
                if (options.onClear !== undefined) {
                    options.onClear();
                }
            });

            el.parent('div').on('click', '.selector-search', function (e) {
                var val = $(this).closest('.input-group').find('input').val();
                if (options.onSearch !== undefined) {
                    options.onSearch(val);
                }
            });


            $(this).data("konseptSelector", konseptSelector);
            return $(this);
        });
    };

    $.fn.konseptSelector.defaultOptions = {
        timesIcon: 'fa-remove'
    };
})(jQuery);