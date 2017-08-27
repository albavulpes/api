// Gulp
const gulp = require('gulp');
const plugins = require('gulp-load-plugins')();
const path = require('path');
const del = require('del');
const runSequence = require('run-sequence');

// Directories
const paths = {};

paths.root = path.join(__dirname, '.');
paths.src = path.join(paths.root, 'src');
paths.dist = path.join(paths.root, 'dist');

// Default Task
gulp.task('default', ['build']);

// Build App
gulp.task('build', function (callback) {
    runSequence(
        'clean',
        'compile',
        callback
    );
});

// Clean
gulp.task('clean', function (callback) {
    const delPaths = [
        path.join(paths.dist, '**')
    ];
    
    return del(delPaths, {force: true}, callback);
});

// Compile TS files
gulp.task('compile', function () {
    const compileSrc = path.join(paths.src, '**', '*.ts');
    const compileDest = paths.dist;
    const tsConfigPath = path.join(paths.root, 'tsconfig.json');
    
    const tsProject = plugins.typescript.createProject(tsConfigPath);
    
    return gulp.src(compileSrc)
        .pipe(plugins.cached('ts_source'))
        .pipe(plugins.debug({title: 'compiled:'}))
        .pipe(plugins.sourcemaps.init())
        .pipe(tsProject())
        .pipe(plugins.sourcemaps.write('.'))
        .pipe(gulp.dest(compileDest));
});

// Watch
gulp.task('watch', ['compile', 'copy-files'], function () {
    const filesToWatch = path.join(paths.src, '**');
    
    plugins.watch(filesToWatch, () => {
        runSequence(['compile', 'copy-files']);
    });
});


