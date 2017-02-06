const path = require("path");
const merge = require("merge-stream");

module.exports = function (gulp, plugins, paths, project)
{    
    // Compile Server files
    gulp.task("compile", function ()
    {
        var serverDest = paths.build;
        var tsConfigPath = path.join(paths.root, "tsconfig.json");
        
        var tsProject = plugins.typescript.createProject(tsConfigPath);
        
        var tsResult = tsProject.src()
            .pipe(plugins.cached("ts-server"))
            .pipe(tsProject());
        
        var tsTask = tsResult
            .pipe(plugins.debug({title: "compiled:"}))
            .pipe(gulp.dest(serverDest));
        
        var filesToCopy = [
            `${paths.src}/**/*`,
            `!${paths.src}/**/*.ts`
        ];
        
        var copyTask = gulp.src(filesToCopy)
            .pipe(gulp.dest(serverDest));
        
        return merge(copyTask, tsTask);
    });
};
