/**
 * Build process for YetAnotherForum.NET
 *
 * Don't know where to start?
 * Try: http://24ways.org/2013/grunt-is-not-weird-and-hard/
 */

module.exports = function(grunt) {

    // CONFIGURATION
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        // Minimize JS
        uglify: {
            codeMirror: {
                options: {
                    sourceMap: false,
                    output: { beautify: true },
                    mangle: false,
                    compress: false
                },
                src: [
                    'node_modules/codemirror/lib/codemirror.js',
                    'node_modules/codemirror/mode/xml/xml.js',
                    'node_modules/codemirror/addon/edit/matchbrackets.js',
                    'node_modules/codemirror/addon/hint/show-hint.js',
                    'node_modules/codemirror/addon/hint/xml-hint.js'
                ],
                dest: 'wwwroot/js/codemirror.js'
            },
            minify: {
                files: {
                    "wwwroot/js/codemirror.js": 'wwwroot/js/codemirror.js'

                }
            }
        },

        // CSS Minify
        cssmin: {
            codeMirror: {
                files: {
                    "wwwroot/css/codemirror.css": [
                        'node_modules/codemirror/lib/codemirror.css',
                        'node_modules/codemirror/theme/blackboard.css',
                        'node_modules/codemirror/addon/hint/show-hint.css'
                    ]
                }
            }
        },

        devUpdate: {
            main: {
                options: {
                    reportUpdated: true,
                    updateType: 'force',
                    semver: true,
                    packages: {
                        devDependencies: true,
                        dependencies: true
                    }
                }
            }
        }
    });

    // PLUGINS
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('@w8tcha/grunt-dev-update');

    grunt.registerTask('default',
        [
            'uglify', 'cssmin'
        ]);
};
