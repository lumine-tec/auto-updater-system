module.exports = (app) => {
    const fs = require("fs");

    // [GET] => /version
    app.get(`/version`, (req, res) => {
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

    // [GET] => /version/:version
    app.get(`/version/:version`, (req, res) => {
        const { version } = req.params;

        const packages = fs.readdirSync(`${ROOT}/versions`);
        const file = `${ROOT}/versions/${version}/program.exe`;

        if (!packages.includes(version) || !fs.existsSync(file)) {
            res.status(404).json("Version not found!");
            return;
        }

        res.download(file);
    });

};