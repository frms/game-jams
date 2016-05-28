var fs = require('fs');
var vm = require('vm');

function include(path) {
    var code = fs.readFileSync(path, 'utf-8');
    vm.runInThisContext(code, path);
}

include('./mapSections.js');

console.dir(sections);

for(s in sections) {
	for(r in sections[s]) {
		for(c in sections[s][r]) {
			sections[s][r][c] = {tile:sections[s][r][c], wall:0};
		}
	}
}

fs.writeFile("./temp.json", JSON.stringify(sections), function(err) {
    if(err) {
      console.log(err);
    } else {
      console.log("JSON saved to ");
    }
}); 