import { observer } from 'mobx-react-lite'
import { SyntheticEvent, useState } from 'react'
import { Button, Card, Grid, Header, Image, Tab } from 'semantic-ui-react'
import { Profile } from '../../app/models/profile'
import { useMainStore } from '../../app/stores/mainStore'
import PhotoUploadWidget from '../../app/common/imageUpload/PhotoUploadWidget'
import Photo from '../../app/models/photo'

interface Props {
  profile: Profile
}

export default observer(function ProfilePhotos(props: Props) {
  const { profile } = props
  const { profileStore } = useMainStore()
  const {
    isCurrentUser,
    uploadPhoto,
    uploadingFile,
    loading,
    setProfilePhoto,
    deletePhoto,
  } = profileStore

  const [addPhotoMode, setAddPhotoMode] = useState<boolean>(false)
  const [target, setTarget] = useState<number>()

  async function handlePhotoUpload(file: Blob) {
    await uploadPhoto(file)
    setAddPhotoMode(false)
  }

  function handleSetProfilePhoto(
    photo: Photo,
    e: SyntheticEvent<HTMLButtonElement>
  ) {
    setTarget(Number(e.currentTarget.name))
    setProfilePhoto(photo)
  }

  function handleDeletePhoto(
    photo: Photo,
    e: SyntheticEvent<HTMLButtonElement>
  ) {
    setTarget(Number(e.currentTarget.name))
    deletePhoto(photo)
  }

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16}>
          <Header floated="left" icon="image" content="Photos" />
          {isCurrentUser && (
            <Button
              floated="right"
              basic
              content={addPhotoMode ? 'Cancel' : 'Add Photo'}
              onClick={() => setAddPhotoMode(!addPhotoMode)}
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {addPhotoMode ? (
            <PhotoUploadWidget
              loading={uploadingFile}
              uploadPhoto={handlePhotoUpload}
            />
          ) : (
            <Card.Group itemsPerRow={5}>
              {profile.photos?.map((photo) => (
                <Card key={photo.id}>
                  <Image src={photo.url} />
                  {isCurrentUser && (
                    <Button.Group fluid widths={2}>
                      <Button
                        basic
                        color="green"
                        content="Set as profile photo"
                        name={photo.id}
                        disabled={loading}
                        loading={target === photo.id && loading}
                        onClick={(e) => handleSetProfilePhoto(photo, e)}
                      />
                      <Button
                        basic
                        color="red"
                        icon="trash"
                        name={photo.id}
                        disabled={loading}
                        loading={target === photo.id && loading}
                        onClick={(e) => handleDeletePhoto(photo, e)}
                      />
                    </Button.Group>
                  )}
                </Card>
              ))}
            </Card.Group>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  )
})
