jQuery(function ($) {

    if ($("#dynamic-table").length > 0) {

        AplicaTooltip();

        AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null, { "bSortable": false }], false, 20);
    }

});

