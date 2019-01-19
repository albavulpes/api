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
				PUBLISH_SITE = credentials('Stage_SiteName')
				PUBLISH_MACHINE = credentials('DevPublishMachine')
				PUBLISH_CREDENTIALS = credentials('DevPublishCredentials')
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