'use strict';


//Includes
const https = require('node:https');
const cors = require('cors');
const express = require('express');
const bodyParser = require("body-parser");
const fs = require('fs');


//Constants
const PORT = 8080;
const HOST = '0.0.0.0';
const CORSOrigin = '*'; //Necessary if your game runs in the browser.
const dataDir = './uploads';
const options = {
	key: fs.readFileSync('./ssl/privkey.pem'),
	cert: fs.readFileSync('./ssl/cert.pem'),
};


//App
const app = express();
app.use(
	bodyParser.urlencoded({ extended: false, limit: 132907008 }),
	cors({origin: CORSOrigin})
	);

app.get('/', (req, res) => {
	res.send('This is the Unity analytics server.');
	console.log('Accessed via https Get.');
});

app.post('/', (req, res) => {
	try
	{
		let data = req.body.data;
		
		if (!data)
		{
			throw 'Received package does not contain body.data.';
		}
		
		if (!fs.existsSync(dataDir))
		{
			fs.mkdirSync(dataDir);
		}
	
		let fileName = dataDir + '/' + GetDateTime() + '.txt';
		let stream = fs.createWriteStream(fileName, {encoding: 'utf16le'});
		stream.once('open', function () {
			stream.write(data);
			stream.end();
		});
	
		res.end('File uploaded successfully to "' + fileName + '".');
		console.log('File uploaded successfully to "' + fileName + '".');
	}
	catch (error)
	{
		res.end('Internal serve rerror.');
		console.log(error);
	}
});


//Start the server
let httpsServer = https.createServer(options, app);
httpsServer.listen(PORT);


//End with prompt
console.log(`Running on ${HOST}:${PORT}`);


//Functions
function GetDateTime() {
	let now = new Date();

	return now.getFullYear()
		+ '_'
		+ (now.getMonth() + 1)
		+ '_'
		+ now.getDate()
		+ '_'
		+ now.getHours()
		+ '_'
		+ now.getMinutes()
		+ '_'
		+ now.getSeconds()
		+ '_'
		+ now.getMilliseconds()
}