$(document).ready(function () {
    $('#product-table').DataTable({
        "scrollY": "450px",
        "scrollCollapse": "true",
        "paging": true,
        "bProcessing": true,
        "bLengthChange":true,
        "lengthMenu": [[5, 10, 25,50,60], [5, 10, 25, 50,60]],
        "bfilter":true
    })
})