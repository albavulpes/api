pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
				powershell './build.ps1 -Script build.cake'
            }
        }
		stage('Deploy To Stage') {
			when {
				branch 'develop'
			}
			environment {
				PUBLISH_CREDENTIALS = credentials('DevPublishCredentials')
				PUBLISH_MACHINE = credentials('DevPublishMachine')
			}
			steps {
				powershell './build.ps1 -Script deploy.cake'
			}
		}
    }
	post {
        always {
            archiveArtifacts artifacts: 'release/release.zip', onlyIfSuccessful: true
        }
    }
}