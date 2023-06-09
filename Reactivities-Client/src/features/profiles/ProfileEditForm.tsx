import { Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { Button } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import * as Yup from "yup";
import CustomTextInput from "../../app/common/form/CustomTextInput";
import CustomTextArea from "../../app/common/form/CustomTextArea";

interface Props {
  setEditMode: (editMode: boolean) => void;
}

export default observer(function ProfileEditForm(props: Props) {
  const { setEditMode } = props;
  const { profileStore } = useStore();
  const { profile, updateProfile } = profileStore;

  const validationSchema = Yup.object({
    displayName: Yup.string().required(),
  });

  async function handleSubmit(profile: Partial<Profile>) {
    await updateProfile(profile);
    setEditMode(false);
  }

  return (
    <Formik
      validationSchema={validationSchema}
      initialValues={{
        displayName: profile?.displayName,
        bio: profile?.bio,
      }}
      onSubmit={handleSubmit}
    >
      {({ isSubmitting, isValid, dirty }) => (
        <Form className="ui form">
          <CustomTextInput name="displayName" placeholder="Display Name" />
          <CustomTextArea name="bio" rows={3} placeholder="Add your bio" />
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
  );
});
