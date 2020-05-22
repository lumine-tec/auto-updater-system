module.exports = (app) => {

    // [GET] => /version
    app.get(`/version`, (req, res) => {
        const fs = require("fs");

        let ver = null;

        const packages = fs.readdirSync(`${ROOT}/versions`);

        if (packages.length > 0) {

            packages.sort((a, b) => {
                if (a > b)
                    return -1;
                if (a < b)
                    return 1;

                return 0;
            });

            ver = packages[0];
        }
        
        res.json(ver);
    });


};