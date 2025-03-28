$(function () {
    // Configure Cloudinary
    // with the credentials on
    // your Cloudinary dashboard
    $.cloudinary.config({ cloud_name: 'YOUR_CLOUD_NAME', api_key: 'YOUR_API_KEY' });
    // Upload button
    var uploadButton = $('#submit');
    // Upload-button event
    uploadButton.on('click', function (e) {
        // Initiate upload
        cloudinary.openUploadWidget({ cloud_name: 'YOUR_CLOUD_NAME', upload_preset: 'YOUR_UPLOAD_PRESET', tags: ['cgal'] },
            function (error, result) {
                if (error) console.log(error);
                // If NO error, log image data to console
                var id = result[0].public_id;
                console.log(processImage(id));
            });
    });
})
function processImage(id) {
    var options = {
        client_hints: true,
    };
    return $.cloudinary.url(id, options);
}