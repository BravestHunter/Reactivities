import { observer } from 'mobx-react-lite'
import { Modal } from 'semantic-ui-react'
import { useGlobalStore } from '../../stores/globalStore'

export default observer(function ModalContainer() {
  const { modalStore } = useGlobalStore()

  return (
    <Modal
      open={modalStore.modal.open}
      onClose={modalStore.closeModal}
      size="mini"
    >
      <Modal.Content>{modalStore.modal.body}</Modal.Content>
    </Modal>
  )
})
