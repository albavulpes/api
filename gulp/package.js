const del = require("del");
const merge = require("merge-stream");
const runSequence = require("run-sequence");

module.exports = function(gulp, plugins, paths, project)
{
    gulp.task("clean", function (callback)
    {
        return del(paths.build + "/**", {force: true}, callback);
    });
        
    // Update Assembly Info
    gulp.task("update-package-info", function ()
    {
        // Update package.json Info
        var packageJson = gulp.src(paths.root + "/package.json")
            .pipe(plugins.debug({title: "package.json:"}))
            .pipe(plugins.jsonEditor({
                name: project.meta.name,
                version: project.meta.version,
                description: project.meta.description,
                copyright: project.meta.copyright,
                author: project.meta.author
            }))
            .pipe(gulp.dest(paths.root));
        
        return merge(packageJson);
    });
    
    // Build App
    gulp.task("build", function (callback)
    {
        runSequence(
            "clean",
            "update-package-info",
            "compile",
            callback
        );
    });

    // Package App
    gulp.task("package", ["build"], function ()
    {
        var serverPaths = [
            paths.build + "/**/*",
            paths.root + "/package.json"
        ];
        
        var server = gulp.src(serverPaths)
            .pipe(plugins.zip("server.zip", {compress: true}))
            .pipe(gulp.dest(paths.deploy));
        
        return server;
    });
};
