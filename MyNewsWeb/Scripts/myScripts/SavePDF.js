function savePdfProcess(canvasObj, fileName, callback) {
    var pdf = new jsPDF('l', 'pt', 'a4'),
      pdfConf = {
          pagesplit: false,
          background: '#fff'
      };
    document.body.appendChild(canvasObj); //appendChild is required for html to add page in pdf
    pdf.addHTML(canvasObj, 0, 0, pdfConf, function () {
        document.body.removeChild(canvasObj);
        pdf.addPage();
        pdf.save(fileName + '.pdf');
        callback();
    });
}

$('#savePdf').click(() => {
    html2canvas(document.getElementById('domToPrint'), {
        onrendered: function (canvasObj) {
            savePdfProcess(canvasObj, 'newsPDF', function () {
                alert('PDF saved');
            });
            //save this object to the pdf
        }
    });
})