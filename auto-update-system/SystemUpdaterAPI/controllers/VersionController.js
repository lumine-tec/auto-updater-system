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
        
        res.send(ver);
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

    // [POST] => /version
    app.post(`/version`, (req, res) => {
        try {
            const { files, body } = req;

            if (!files || !files.program) {
                res.status(400).send("You need to send the .exe file!");
                return;
            }

            if (!body.version) {
                res.status(400).send("Version number is mandatory!");
                return;
            }

            const version = `${ROOT}/versions/${body.version}`;

            if (!fs.existsSync(version)) {
                fs.rmdirSync(version);
            } else {
                fs.mkdirSync(version);
                fs.rmdirSync(version);
            }

            let program = files.program;
            program.mv(`${version}/program.exe`);

            res.send("Version uploaded successfully!");

        } catch (err) {
            res.status(500).send(err);
        }


    });

};