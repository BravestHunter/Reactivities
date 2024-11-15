import { ErrorMessage, Form, Formik } from 'formik'
import CustomTextInput from '../../app/common/form/CustomTextInput'
import { Button, Header } from 'semantic-ui-react'
import { useStore } from '../../app/stores/store'
import { observer } from 'mobx-react-lite'
import * as Yup from 'yup'
import ValidationErrors from '../errors/ValidationErrors'

export default observer(function RegisterForm() {
  const { userStore } = useStore()

  return (
    <Formik
      initialValues={{
        username: '',
        displayName: '',
        email: '',
        password: '',
        error: null,
      }}
      onSubmit={(values, { setErrors }) =>
        userStore.register(values).catch((error) => setErrors({ error }))
      }
      validationSchema={Yup.object({
        username: Yup.string().required(),
        displayName: Yup.string().required(),
        email: Yup.string().required(),
        password: Yup.string().required(),
      })}
    >
      {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
        <Form
          className="ui form error"
          onSubmit={handleSubmit}
          autoComplete="off"
        >
          <Header
            as="h2"
            content="Sign up to Reactivities"
            color="teal"
            textAlign="center"
          />
          <CustomTextInput name="username" placeholder="Username" />
          <CustomTextInput name="displayName" placeholder="Display Name" />
          <CustomTextInput name="email" placeholder="Email" />
          <CustomTextInput
            name="password"
            placeholder="Password"
            type="password"
          />
          <ErrorMessage
            name="error"
            render={() => <ValidationErrors errors={errors.error} />}
          />
          <Button
            disabled={!isValid || !dirty || isSubmitting}
            positive
            loading={isSubmitting}
            content="Login"
            type="submit"
            fluid
          />
        </Form>
      )}
    </Formik>
  )
})
