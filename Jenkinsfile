pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
				powershell '.\build.ps1'
            }
        }
    }
}