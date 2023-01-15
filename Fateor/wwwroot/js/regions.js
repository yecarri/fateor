function setRegions(url, countryCtl, regionCtl) {
    var country = $(countryCtl).val();

    $.ajax(url +"/" + country + '/regions', {
        method: 'get',
        beforeSend: function (req) {
            req.setRequestHeader('Authorization', '<?= base64_encode("a:b"); ?>');
        },
        success: function (response, status, jqXHR) {
            var id = $(regionCtl).attr("value");
            $(regionCtl).empty().append("<option value=''>-- cualquiera --</option>");
            $.each(response, function (index, item) {
                $(regionCtl).append("<option value='" + item.RegionId + "' " + (item.RegionId == id ? "selected='selected'" : "") + ">" + item.RegionName + "</option>");
            });
        },
        error: function (error) { alert(error); },
        xhrFields: {
            withCredentials: true
        }
    });
}
