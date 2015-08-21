var evilduck = evilduck || {};

(function (_evilduck) {

    _evilduck.wrapGridFilters = function () {

        $('.filter-number-button').each(function (idx, jqElem) {

            var htmlContent = $('.filter-number', $(jqElem).parent()).html();

            $(jqElem).popover({
                content: htmlContent,
                placement: 'bottom',
                html: true
            });
        });

        $('.filter-string-button').each(function (idx, jqElem) {

            var htmlContent = $('.filter-string', $(jqElem).parent()).html();

            $(jqElem).popover({
                content: htmlContent,
                placement: 'bottom',
                html: true
            });
        });

        $('.filter-enum-button').each(function (idx, jqElem) {

            var htmlContent = $('.filter-enum', $(jqElem).parent()).html();

            $(jqElem).popover({
                content: htmlContent,
                placement: 'bottom',
                html: true
            });
        });

        $('.filter-datetime-button').each(function (idx, jqElem) {

            var htmlContent = $('.filter-datetime', $(jqElem).parent()).html();

            $(jqElem).popover({
                content: htmlContent,
                placement: 'bottom',
                html: true,

            }).on('shown.bs.popover', function () {
                $('.date', $('.popover', $(jqElem).parent())).datetimepicker({
                    locale: 'pl'
                });
            });

        });

    };

    _evilduck.wrapSimpleSelector = function () {
        $('.simple-selector').select2();
    };

    _evilduck.reloadGrid = function () {
        _evilduck.wrapGridFilters();
    };

    _evilduck.wrapAjaxMultiSelector = function (ctrl, queryUrl, itemUrl) {
        $(ctrl).select2({
            placeholder: "Wyszukaj...",
            minimumInputLength: 2,
            multiple: true,
            ajax: {
                url: queryUrl,
                dataType: 'json',
                quietMillis: 250,
                data: function (term) {
                    return {
                        q: term
                    };
                },
                results: function (data) {
                    var res = { results: [] };
                    for (var i = 0; i < data.length; i++) {
                        var r = {
                            id: data[i].Id,
                            text: data[i].Name
                        };

                        res.results.push(r);
                    }

                    return res;
                }
            },

            initSelection: function (element, callback) {
                var val = element.val();

                var idsStrArr = val.split(',');

                var promises = _.map(idsStrArr, function (idStr) {

                    var d = Q.defer();

                    var id = parseInt(idStr);
                    $.getJSON(itemUrl + "/" + id).done(function (data) {
                        d.resolve(data);
                    }).fail(function (err) {
                        d.reject(err);
                    });

                    return d.promise;
                });

                Q.all(promises).then(function (data) {
                    callback(_.map(data, function (item) {
                        return {
                            id: item.Id,
                            text: item.Name
                        };
                    }));
                });
            },
            formatSearching: function () {
                return "Szukam...";
            },
            formatNoMatches: function () {
                return "Brak wyników";
            },
            formatInputTooShort: function () {
                return "Rozpocznij wpisywanie...";
            }
        });
    };

    _evilduck.wrapAjaxSelector = function (ctrl, queryUrl, itemUrl) {
        $(ctrl).select2({
            placeholder: "Wyszukaj...",
            minimumInputLength: 2,
            ajax: {
                url: queryUrl,
                dataType: 'json',
                quietMillis: 250,
                data: function (term) {
                    return {
                        q: term
                    };
                },
                results: function (data) {
                    var res = { results: [] };
                    for (var i = 0; i < data.length; i++) {
                        var r = {
                            id: data[i].Id,
                            text: data[i].Name
                        };

                        res.results.push(r);
                    }

                    return res;
                }
            },
            initSelection: function (element, callback) {
                var val = element.val();

                $.getJSON(itemUrl + "/" + val).done(function (data) {
                    callback({ id: data.Id, text: data.Name });
                });
            },
            formatSearching: function () {
                return "Szukam...";
            },
            formatNoMatches: function () {
                return "Brak wyników";
            },
            formatInputTooShort: function () {
                return "Rozpocznij wpisywanie...";
            }
        });
    };

})(evilduck);

evilduck.reloadGrid();
//lopor.reloadEditor();
