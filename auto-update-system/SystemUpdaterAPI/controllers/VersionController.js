module.exports = (app) => {

    // [GET] => /version/current
    app.get(`/version/current`, (req, res) => {
        const fs = require("fs");

        const json = JSON.parse(fs.readFileSync(`${ROOT}/versions/versions.json`).toString('utf8'));

        let ver = null;

        if(json.length > 0){

            json.sort((a, b) => {
                var versionA = a.version.toLowerCase(), versionB = b.version.toLowerCase();
                if (versionA > versionB)
                    return -1;
                if (versionA < versionB)
                    return 1;
                return 0;
            });

            ver = json[0];

        }
        
        
        res.send(ver);
    });


};