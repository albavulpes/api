pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
				powershell './build.ps1'
            }
        }
    }
	post {
        always {
            archiveArtifacts artifacts: './dist/**/*', onlyIfSuccessful: true
        }
    }
}