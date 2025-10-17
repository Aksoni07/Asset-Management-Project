// This function takes a filename and Base64 encoded content,
// and triggers a download in the browser.
function saveAsFile(fileName, byteBase64) {
    var link = document.createElement('a'); // invisible link in browser 
    link.download = fileName; // download the content of the link's href
    link.href = "data:application/octet-stream;base64," + byteBase64;
    document.body.appendChild(link); //to make part of webpage
    link.click(); // save as...
    document.body.removeChild(link);
}