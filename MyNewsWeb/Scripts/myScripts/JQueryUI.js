$(function () {
    //date picker ui
    $("#dobDatePicker").datepicker({ dateFormat: 'dd/mm/yy' });
    $("#dobDatePicker2").datepicker({ dateFormat: 'dd/mm/yy' });

    //password match check
    $("#passwordverify").keyup(validate);

    //data table search
    var tableN = $('#newsid').DataTable( {
        initComplete: function () {
            this.api().columns().every( function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo( $(column.footer()).empty() )
                    .on( 'change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
 
                        column
                            .search( val ? '^'+val+'$' : '', true, false )
                            .draw();
                    } );
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });

    $('.dataTables_filter input').unbind().bind('keyup', function () {
        var colIndex = document.querySelector('#selectNews').selectedIndex;
        tableN.column(colIndex).search(this.value).draw();

    });

});

function validate() {
    var password1 = $("#passwordin").val();
    var password2 = $("#passwordverify").val();

    if (password1 != password2) {
        $("#validate-status").show();
        $("#validate-status").text("Confirmed password must be same as password typed by user!");
    } else {
        $("#validate-status").hide();
    }
}