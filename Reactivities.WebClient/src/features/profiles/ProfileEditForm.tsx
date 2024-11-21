import { Form, Formik } from 'formik'
import { observer } from 'mobx-react-lite'
import { Button } from 'semantic-ui-react'
import * as Yup from 'yup'
import FormTextInput from '../../app/common/form/FormTextInput'
import FormTextArea from '../../app/common/form/FormTextArea'
import ProfileFormValues from '../../app/models/forms/profileFormValues'
import { useProfileStore } from '../../app/stores/profileStore'

interface Props {
  setEditMode: (editMode: boolean) => void
}

export default observer(function ProfileEditForm(props: Props) {
  const { setEditMode } = props
  const profileStore = useProfileStore()
  const { profile, updateProfile } = profileStore

  const validationSchema = Yup.object({
    displayName: Yup.string().required(),
  })

  async function handleSubmit(profile: ProfileFormValues) {
    await updateProfile(profile)
    setEditMode(false)
  }

  return (
    <Formik
      validationSchema={validationSchema}
      initialValues={{
        displayName: profile?.displayName ?? '',
        bio: profile?.bio,
      }}
      onSubmit={handleSubmit}
    >
      {({ isSubmitting, isValid, dirty }) => (
        <Form className="ui form">
          <FormTextInput name="displayName" placeholder="Display Name" />
          <FormTextArea name="bio" rows={3} placeholder="Add your bio" />
          <Button
            positive
            type="submit"
            loading={isSubmitting}
            content="Update profile"
            floated="right"
            disabled={!isValid || !dirty}
          />
        </Form>
      )}
    </Formik>
  )
})
