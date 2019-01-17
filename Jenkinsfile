pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
				.\build.ps1 -Script build.cake
            }
        }
		stage('Deploy To Stage') {
			when {
				branch 'develop'
			}
			steps {
				withCredentials([
					[
						$class: 'UsernamePasswordMultiBinding', 
						credentialsId: 'DevPublishCredentials',
						usernameVariable: 'PublishUsername', 
						passwordVariable: 'PublishPassword'
					],
					string(credentialsId: 'DevPublishMachine', variable: 'PublishMachine')
				]) {
					.\build.ps1 -Script deploy.cake ^
					--ScriptArgs '--machine="$PublishMachine"','--username="$PublishUsername"','--password="PublishPassword"'
				}
			}
		}
    }
	post {
        always {
            archiveArtifacts artifacts: 'release/release.zip', onlyIfSuccessful: true
        }
    }
}