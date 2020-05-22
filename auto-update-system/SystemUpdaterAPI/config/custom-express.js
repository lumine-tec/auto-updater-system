const express = require("express");
const consign = require("consign");
const bodyparser = require("body-parser");
const expressValidator = require("express-validator");
const referrerPolicy = require('referrer-policy');
var cors = require('cors')

var engines = require('consolidate');


module.exports = function(){
    var app = express();

    app.use(cors())
    app.use(bodyparser.urlencoded({extended: true}));
    app.use(bodyparser.json());
    app.use(expressValidator());
    app.use(referrerPolicy({ policy: "strict-origin" }));

    app.use(function(req, res, next) {
        res.header("Access-Control-Allow-Origin", "*");
        res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
        res.header('Access-Control-Allow-Methods', 'PUT, POST, GET, DELETE, OPTIONS');
        
        res.return = (e) => {
            
            res.send(e);
        };

        next();
    });

    consign().include("controllers").into(app);

    return app;
}