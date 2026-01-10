const path = require('path');
const TerserPlugin = require('terser-webpack-plugin');

const pckg = require('./package.json');

module.exports = [
	{
		entry: {
			'codemirror-xml': './wwwroot/lib/codemirror-xml.ts',
			'codemirror-json': './wwwroot/lib/codemirror-json.ts'
		},
		output: {
			filename: '[name].min.js',
			path: path.resolve(__dirname, 'wwwroot/js/'),
			clean: false
		},
		devtool: 'source-map',
		mode: 'production',
		resolve: {
			extensions: ['.ts', '.js'],
			extensionAlias: { '.js': ['.js', '.ts'] }
		},
		optimization: {
			minimize: true,
			minimizer: [new TerserPlugin({
				terserOptions: {
					format: {
						comments: false
					}
				}, extractComments: false })]
		},
		stats: {
			orphanModules: true
		},
		module: {
			rules: [
				{
					test: /\.css$/i,
					use: ['style-loader', 'css-loader']
				},
				{
					test: /\.ts$/i,
					use: ['ts-loader'],
					exclude: /node_modules/
				}
			]
		}
	}
];
