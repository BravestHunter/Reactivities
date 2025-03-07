import { observer, useLocalObservable } from 'mobx-react-lite'
import { useEffect } from 'react'
import { Segment, Comment, Header, Loader } from 'semantic-ui-react'
import { useMainStore } from '../../../app/stores/mainStore'
import { Link } from 'react-router-dom'
import { Field, FieldProps, Form, Formik } from 'formik'
import * as Yup from 'yup'
import { formatDistanceToNow } from 'date-fns'
import CommentStore from '../../../app/stores/commentsStore'

interface Props {
  activityId: number
}

export default observer(function ActivityDetailedChat(props: Props) {
  const { activityId } = props
  const { activityStore } = useMainStore()
  const commentStore = useLocalObservable(() => new CommentStore(activityStore))

  useEffect(() => {
    if (activityId) {
      commentStore.createHubConnection(activityId)
    }
    return () => {
      commentStore.clearComments()
    }
  }, [commentStore, activityId])

  return (
    <>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: 'none' }}
      >
        <Header>Chat about this event</Header>
      </Segment>
      <Segment attached clearing>
        <Formik
          onSubmit={(values, { resetForm }) =>
            commentStore.addComment(values).then(() => resetForm())
          }
          initialValues={{ body: '' }}
          validationSchema={Yup.object({
            body: Yup.string().required(),
          })}
        >
          {({ isSubmitting, isValid, handleSubmit }) => (
            <Form className="ui form">
              <Field name="body">
                {(props: FieldProps) => (
                  <div style={{ position: 'relative' }}>
                    <Loader active={isSubmitting} />
                    <textarea
                      placeholder="Enter your comment (Enter to submit, SHIFT + enter for new line)"
                      rows={2}
                      {...props.field}
                      onKeyPress={(e) => {
                        if (e.key === 'Enter' && e.shiftKey) {
                          return
                        } else if (e.key === 'Enter') {
                          e.preventDefault()
                          isValid && handleSubmit()
                        }
                      }}
                    />
                  </div>
                )}
              </Field>
            </Form>
          )}
        </Formik>

        <Comment.Group>
          {commentStore.comments.map((comment) => (
            <Comment key={comment.id}>
              <Comment.Avatar
                src={comment.profilePhotoUrl || '/assets/user.png'}
              />
              <Comment.Content>
                <Comment.Author as={Link} to={`/profiles/${comment.username}`}>
                  {comment.displayName}
                </Comment.Author>
                <Comment.Metadata>
                  <div>{formatDistanceToNow(comment.createdAt)} ago</div>
                </Comment.Metadata>
                <Comment.Text style={{ whiteSpace: 'pre-wrap' }}>
                  {comment.body}
                </Comment.Text>
              </Comment.Content>
            </Comment>
          ))}
        </Comment.Group>
      </Segment>
    </>
  )
})
