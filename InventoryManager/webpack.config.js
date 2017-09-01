/**
 * Created by Design on 7/31/2017.
 */

var path = require('path');
var ExtractTextPlugin = require("extract-text-webpack-plugin");

module.exports = {
    entry : {
        app: './src/app.js',
    },
    output : {
        path: __dirname + '/Content',
        filename: '[name].bundle.js'
    },
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: ["css-loader", "sass-loader"],
                })
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: "babel-loader"
            }
        ],

    },
    plugins: [
        new ExtractTextPlugin("[name].styles.css")
    ]
};
