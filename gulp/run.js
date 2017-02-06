const path = require("path");

module.exports = function (gulp, plugins, paths, project)
{
    gulp.task("start", ["compile"], function (callback)
    {
        var serverPath = paths.build;
        
        plugins.nodemon({
            script: serverPath + "/Server.js",
            watch: [serverPath]
        });
        
        gulp.watch(`${paths.src}/**/*`, ["compile"]);
    });
};
