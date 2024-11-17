import { useEffect, useState } from 'react'
import { Button, Header, Segment } from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store'
import { observer } from 'mobx-react-lite'
import { Link, useNavigate, useParams } from 'react-router-dom'
import LoadingComponent from '../../../app/layout/LoadingComponent'
import { Form, Formik } from 'formik'
import * as Yup from 'yup'
import CustomTextInput from '../../../app/common/form/CustomTextInput'
import CustomTextArea from '../../../app/common/form/CustomTextArea'
import CustomSelectInput from '../../../app/common/form/CustomSelectInput'
import { categoryOptions } from '../../../app/common/options/categoryOptions'
import CustomDateInput from '../../../app/common/form/CustomDateInput'
import ActivityFormValues from '../../../app/models/forms/activityFormValues'

export default observer(function ActivityForm() {
  const { activityStore } = useStore()
  const { loadActivity, createActivity, updateActivity, loadingInitial } =
    activityStore
  const { id } = useParams()
  const navigate = useNavigate()

  const [activity, setActivity] = useState<ActivityFormValues>(
    new ActivityFormValues()
  )

  const validationSchema = Yup.object({
    title: Yup.string().required('The activity title is required'),
    description: Yup.string().required('The activity description is required'),
    category: Yup.string().required(),
    date: Yup.string().required('Date is required'),
    venue: Yup.string().required(),
    city: Yup.string().required(),
  })

  useEffect(() => {
    if (id)
      loadActivity(id).then((activity) =>
        setActivity(new ActivityFormValues(activity))
      )
  }, [id, loadActivity])

  async function handleFormSubmit(activity: ActivityFormValues) {
    if (!activity.id) {
      await createActivity(activity)
    } else {
      await updateActivity(activity)
    }

    navigate(`/activities/${activity.id}`)
  }

  if (loadingInitial) return <LoadingComponent content="Loading activity..." />

  return (
    <Segment clearing>
      <Header content="Activity Details" sub color="teal" />
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={activity}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <CustomTextInput name="title" placeholder="Title" />
            <CustomTextArea
              name="description"
              placeholder="Description"
              rows={3}
            />
            <CustomSelectInput
              name="category"
              placeholder="Category"
              options={categoryOptions}
            />
            <CustomDateInput
              name="date"
              placeholderText="Date"
              showTimeSelect
              dateFormat="MMMM d, yyyy h:mm aa"
            />
            <Header content="Location Details" sub color="teal" />
            <CustomTextInput name="city" placeholder="City" />
            <CustomTextInput name="venue" placeholder="Venue" />
            <Button
              disabled={isSubmitting || !dirty || !isValid}
              loading={isSubmitting}
              floated="right"
              positive
              type="submit"
              content="Submit"
            />
            <Button
              as={Link}
              to="/activities"
              floated="right"
              type="button"
              content="Cancel"
            />
          </Form>
        )}
      </Formik>
    </Segment>
  )
})
